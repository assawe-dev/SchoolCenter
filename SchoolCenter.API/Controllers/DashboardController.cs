using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace SchoolCenter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connStr))
            {
                return "Server=.\\SQLEXPRESS;Database=SchoolCenterDB;Integrated Security=True;TrustServerCertificate=True";
            }
            return connStr;
        }

        [HttpGet("stats")]
        public IActionResult GetStats()
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
                    totalStudents,
                    totalCourses,
                    currentTreasuryBalance,
                    totalOutstandingDebts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء جلب إحصائيات لوحة التحكم", error = ex.Message });
            }
        }

        [HttpGet("donut-chart")]
        public IActionResult GetDonutChartData()
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
                    totalPaid,
                    totalOutstanding,
                    labels = new string[] { "المدفوعات المستلمة", "الديون المستحقة" }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء جلب بيانات المخطط الدائري", error = ex.Message });
            }
        }

        [HttpGet("recent-transactions")]
        public IActionResult GetRecentTransactions()
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
                return StatusCode(500, new { message = "حدث خطأ أثناء جلب الحركات المالية الأخيرة", error = ex.Message });
            }
        }
    }
}
