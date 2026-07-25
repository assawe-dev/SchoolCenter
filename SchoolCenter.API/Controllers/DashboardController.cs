using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SchoolCenter.API.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        private string GetConnectionString()
        {
            var connStrSetting = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (connStrSetting != null && !string.IsNullOrEmpty(connStrSetting.ConnectionString))
            {
                return connStrSetting.ConnectionString;
            }

            // Fallback to db_config.txt in the application directory or parent directory
            try
            {
                string configPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "db_config.txt");
                if (!System.IO.File.Exists(configPath))
                {
                    configPath = System.IO.Path.Combine(System.IO.Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).FullName, "db_config.txt");
                }

                if (System.IO.File.Exists(configPath))
                {
                    var builder = new SqlConnectionStringBuilder();
                    string[] lines = System.IO.File.ReadAllLines(configPath);
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
                            continue;
                        int delimiterIndex = line.IndexOf('=');
                        if (delimiterIndex > 0)
                        {
                            string key = line.Substring(0, delimiterIndex).Trim().ToUpper();
                            string value = line.Substring(delimiterIndex + 1).Trim();
                            switch (key)
                            {
                                case "SERVER":
                                case "DATA SOURCE":
                                    builder.DataSource = value;
                                    break;
                                case "DATABASE":
                                case "INITIAL CATALOG":
                                    builder.InitialCatalog = value;
                                    break;
                                case "INTEGRATED_SECURITY":
                                case "INTEGRATED SECURITY":
                                    bool integrated;
                                    if (bool.TryParse(value, out integrated))
                                        builder.IntegratedSecurity = integrated;
                                    break;
                                case "USER ID":
                                    builder.UserID = value;
                                    break;
                                case "PASSWORD":
                                    builder.Password = value;
                                    break;
                            }
                        }
                    }
                    builder.ConnectTimeout = 15;
                    builder.Pooling = true;
                    return builder.ConnectionString;
                }
            }
            catch { }

            return "Server=.\\SQLEXPRESS;Database=SchoolCenterDB;Integrated Security=True;";
        }

        [HttpGet]
        [Route("stats")]
        public IHttpActionResult GetStats()
        {
            try
            {
                int totalStudents = 0;
                int totalCourses = 0;
                decimal currentTreasuryBalance = 0m;
                decimal totalOutstandingDebts = 0m;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Total Students
                    string queryStudents = "SELECT COUNT(*) FROM Students";
                    using (SqlCommand cmd = new SqlCommand(queryStudents, conn))
                    {
                        totalStudents = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Total Courses
                    string queryCourses = "SELECT COUNT(*) FROM Courses";
                    using (SqlCommand cmd = new SqlCommand(queryCourses, conn))
                    {
                        totalCourses = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Current Treasury Balance
                    string queryTreasury = "SELECT ISNULL(SUM(Credit), 0) FROM FinancialTransactions WHERE TransactionType = 'Payment Receipt'";
                    using (SqlCommand cmd = new SqlCommand(queryTreasury, conn))
                    {
                        currentTreasuryBalance = Convert.ToDecimal(cmd.ExecuteScalar());
                    }

                    // Total Outstanding Debts: ISNULL(SUM(Debit), 0) - ISNULL(SUM(Credit), 0)
                    string queryDebts = "SELECT ISNULL(SUM(Debit), 0) - ISNULL(SUM(Credit), 0) FROM FinancialTransactions";
                    using (SqlCommand cmd = new SqlCommand(queryDebts, conn))
                    {
                        totalOutstandingDebts = Convert.ToDecimal(cmd.ExecuteScalar());
                    }
                }

                return Ok(new
                {
                    totalStudents = totalStudents,
                    totalCourses = totalCourses,
                    currentTreasuryBalance = currentTreasuryBalance,
                    totalOutstandingDebts = totalOutstandingDebts
                });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء جلب إحصائيات لوحة التحكم", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("donut-chart")]
        public IHttpActionResult GetDonutChartData()
        {
            try
            {
                decimal totalPaid = 0m;
                decimal totalOutstanding = 0m;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Total Paid
                    string queryPaid = "SELECT ISNULL(SUM(Credit), 0) FROM FinancialTransactions WHERE TransactionType = 'Payment Receipt'";
                    using (SqlCommand cmd = new SqlCommand(queryPaid, conn))
                    {
                        totalPaid = Convert.ToDecimal(cmd.ExecuteScalar());
                    }

                    // Total Outstanding Debts (Remaining)
                    string queryOutstanding = "SELECT ISNULL(SUM(Debit), 0) - ISNULL(SUM(Credit), 0) FROM FinancialTransactions";
                    using (SqlCommand cmd = new SqlCommand(queryOutstanding, conn))
                    {
                        totalOutstanding = Convert.ToDecimal(cmd.ExecuteScalar());
                    }
                }

                // Normalizing to avoid negative remaining debts
                if (totalOutstanding < 0) totalOutstanding = 0;

                return Ok(new
                {
                    totalPaid = totalPaid,
                    totalOutstanding = totalOutstanding,
                    labels = new string[] { "المدفوعات المستلمة", "الديون المستحقة" }
                });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء جلب بيانات المخطط الدائري", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("recent-transactions")]
        public IHttpActionResult GetRecentTransactions()
        {
            try
            {
                var list = new List<object>();

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"
                        SELECT TOP 10
                            ft.TransactionID,
                            s.StudentName,
                            ft.TransactionType,
                            ft.Debit,
                            ft.Credit,
                            ft.TransactionDate,
                            ft.Notes
                        FROM FinancialTransactions ft
                        INNER JOIN Students s ON ft.StudentID = s.StudentID
                        ORDER BY ft.TransactionDate DESC, ft.TransactionID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new
                                {
                                    transactionID = Convert.ToInt32(reader["TransactionID"]),
                                    studentName = reader["StudentName"].ToString(),
                                    transactionType = reader["TransactionType"].ToString(),
                                    debit = Convert.ToDecimal(reader["Debit"]),
                                    credit = Convert.ToDecimal(reader["Credit"]),
                                    transactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                                    notes = reader["Notes"].ToString()
                                });
                            }
                        }
                    }
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء جلب الحركات المالية الأخيرة", error = ex.Message });
            }
        }
    }
}
