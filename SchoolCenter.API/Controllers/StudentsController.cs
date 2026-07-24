using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SchoolCenter.API.Controllers
{
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
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
        [Route("")]
        public IHttpActionResult GetStudents(string search = null)
        {
            try
            {
                var list = new List<StudentResponse>();

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"
                        SELECT StudentID, StudentName, GuardianName, ParentPhone, RegistrationDate, Notes
                        FROM Students";

                    if (!string.IsNullOrEmpty(search))
                    {
                        query += " WHERE StudentName LIKE @Filter OR ParentPhone LIKE @Filter OR GuardianName LIKE @Filter";
                    }

                    query += " ORDER BY StudentID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(search))
                        {
                            cmd.Parameters.AddWithValue("@Filter", "%" + search.Trim() + "%");
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new StudentResponse
                                {
                                    StudentID = Convert.ToInt32(reader["StudentID"]),
                                    StudentName = reader["StudentName"].ToString() ?? string.Empty,
                                    GuardianName = reader["GuardianName"].ToString() ?? string.Empty,
                                    ParentPhone = reader["ParentPhone"].ToString() ?? string.Empty,
                                    RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                                    Notes = reader["Notes"].ToString() ?? string.Empty
                                });
                            }
                        }
                    }

                    // Populate opening balance for each student
                    foreach (var student in list)
                    {
                        string queryOb = "SELECT Debit, Credit FROM FinancialTransactions WHERE StudentID = @StudentID AND TransactionType = 'Opening Balance'";
                        using (SqlCommand cmdOb = new SqlCommand(queryOb, conn))
                        {
                            cmdOb.Parameters.AddWithValue("@StudentID", student.StudentID);
                            using (SqlDataReader obReader = cmdOb.ExecuteReader())
                            {
                                if (obReader.Read())
                                {
                                    decimal debit = Convert.ToDecimal(obReader["Debit"]);
                                    decimal credit = Convert.ToDecimal(obReader["Credit"]);
                                    if (debit > 0)
                                    {
                                        student.OpeningBalanceAmount = debit;
                                        student.BalanceType = "Debit";
                                    }
                                    else if (credit > 0)
                                    {
                                        student.OpeningBalanceAmount = credit;
                                        student.BalanceType = "Credit";
                                    }
                                }
                            }
                        }
                    }
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء جلب قائمة الطلاب", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        public IHttpActionResult GetStudent(int id)
        {
            try
            {
                StudentResponse student = null;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"
                        SELECT StudentID, StudentName, GuardianName, ParentPhone, RegistrationDate, Notes
                        FROM Students
                        WHERE StudentID = @StudentID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                student = new StudentResponse
                                {
                                    StudentID = Convert.ToInt32(reader["StudentID"]),
                                    StudentName = reader["StudentName"].ToString() ?? string.Empty,
                                    GuardianName = reader["GuardianName"].ToString() ?? string.Empty,
                                    ParentPhone = reader["ParentPhone"].ToString() ?? string.Empty,
                                    RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                                    Notes = reader["Notes"].ToString() ?? string.Empty
                                };
                            }
                        }
                    }

                    if (student != null)
                    {
                        string queryOb = "SELECT Debit, Credit FROM FinancialTransactions WHERE StudentID = @StudentID AND TransactionType = 'Opening Balance'";
                        using (SqlCommand cmdOb = new SqlCommand(queryOb, conn))
                        {
                            cmdOb.Parameters.AddWithValue("@StudentID", student.StudentID);
                            using (SqlDataReader obReader = cmdOb.ExecuteReader())
                            {
                                if (obReader.Read())
                                {
                                    decimal debit = Convert.ToDecimal(obReader["Debit"]);
                                    decimal credit = Convert.ToDecimal(obReader["Credit"]);
                                    if (debit > 0)
                                    {
                                        student.OpeningBalanceAmount = debit;
                                        student.BalanceType = "Debit";
                                    }
                                    else if (credit > 0)
                                    {
                                        student.OpeningBalanceAmount = credit;
                                        student.BalanceType = "Credit";
                                    }
                                }
                            }
                        }
                    }
                }

                if (student == null)
                {
                    return Content(System.Net.HttpStatusCode.NotFound, new { message = "الطالب غير موجود" });
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء جلب بيانات الطالب", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id:int}/balance")]
        public IHttpActionResult GetStudentBalance(int id)
        {
            try
            {
                decimal balance = 0m;
                bool studentExists = false;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Check student exists
                    string checkQuery = "SELECT COUNT(*) FROM Students WHERE StudentID = @StudentID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@StudentID", id);
                        studentExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                    }

                    if (!studentExists)
                    {
                        return Content(System.Net.HttpStatusCode.NotFound, new { message = "الطالب غير موجود" });
                    }

                    // Calculate outstanding balance: ISNULL(SUM(Debit), 0) - ISNULL(SUM(Credit), 0)
                    string query = "SELECT ISNULL(SUM(Debit), 0) - ISNULL(SUM(Credit), 0) FROM FinancialTransactions WHERE StudentID = @StudentID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", id);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            balance = Convert.ToDecimal(result);
                        }
                    }
                }

                return Ok(new { studentID = id, outstandingBalance = balance });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء احتساب رصيد الطالب", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateStudent([FromBody] StudentRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.StudentName) || string.IsNullOrWhiteSpace(request.ParentPhone))
            {
                return BadRequest("يرجى إدخال اسم الطالب ورقم هاتف ولي الأمر");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            int newStudentID = -1;
                            string query = "INSERT INTO Students (StudentName, GuardianName, ParentPhone, RegistrationDate, Notes) VALUES (@StudentName, @GuardianName, @ParentPhone, @RegistrationDate, @Notes); SELECT SCOPE_IDENTITY();";
                            using (SqlCommand cmd = new SqlCommand(query, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@StudentName", request.StudentName.Trim());
                                cmd.Parameters.AddWithValue("@GuardianName", request.GuardianName != null ? request.GuardianName.Trim() : string.Empty);
                                cmd.Parameters.AddWithValue("@ParentPhone", request.ParentPhone.Trim());
                                cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@Notes", request.Notes != null ? request.Notes.Trim() : string.Empty);
                                object result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    newStudentID = Convert.ToInt32(result);
                                }
                            }

                            if (request.OpeningBalanceAmount > 0 && newStudentID != -1)
                            {
                                decimal debit = 0;
                                decimal credit = 0;
                                if (!string.IsNullOrEmpty(request.BalanceType) && request.BalanceType.Equals("Debit", StringComparison.OrdinalIgnoreCase))
                                {
                                    debit = request.OpeningBalanceAmount;
                                }
                                else
                                {
                                    credit = request.OpeningBalanceAmount;
                                }

                                string queryTx = @"
                                    INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID)
                                    VALUES (@StudentID, 'Opening Balance', @Debit, @Credit, @TransactionDate, @Notes, @UserID)";
                                using (SqlCommand cmdTx = new SqlCommand(queryTx, conn, trans))
                                {
                                    cmdTx.Parameters.AddWithValue("@StudentID", newStudentID);
                                    cmdTx.Parameters.AddWithValue("@Debit", debit);
                                    cmdTx.Parameters.AddWithValue("@Credit", credit);
                                    cmdTx.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                                    cmdTx.Parameters.AddWithValue("@Notes", "رصيد افتتاح سابق");
                                    cmdTx.Parameters.AddWithValue("@UserID", 1); // default admin user
                                    cmdTx.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                            return CreatedAtRoute("GetStudentById", new { id = newStudentID }, new { studentID = newStudentID, message = "تم إضافة الطالب بنجاح" });
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء حفظ بيانات الطالب والرصيد السابق", error = ex.Message });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ في السيرفر", error = ex.Message });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateStudent(int id, [FromBody] StudentRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.StudentName) || string.IsNullOrWhiteSpace(request.ParentPhone))
            {
                return BadRequest("يرجى إدخال اسم الطالب ورقم هاتف ولي الأمر");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Verify student exists
                    string checkStudentQuery = "SELECT COUNT(*) FROM Students WHERE StudentID = @StudentID";
                    using (SqlCommand checkCmd = new SqlCommand(checkStudentQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@StudentID", id);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                        {
                            return Content(System.Net.HttpStatusCode.NotFound, new { message = "الطالب غير موجود" });
                        }
                    }

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // Update student details
                            string query = "UPDATE Students SET StudentName = @StudentName, GuardianName = @GuardianName, ParentPhone = @ParentPhone, Notes = @Notes WHERE StudentID = @StudentID";
                            using (SqlCommand cmd = new SqlCommand(query, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@StudentName", request.StudentName.Trim());
                                cmd.Parameters.AddWithValue("@GuardianName", request.GuardianName != null ? request.GuardianName.Trim() : string.Empty);
                                cmd.Parameters.AddWithValue("@ParentPhone", request.ParentPhone.Trim());
                                cmd.Parameters.AddWithValue("@Notes", request.Notes != null ? request.Notes.Trim() : string.Empty);
                                cmd.Parameters.AddWithValue("@StudentID", id);
                                cmd.ExecuteNonQuery();
                            }

                            // Update/Insert/Delete opening balance transaction
                            string checkQuery = "SELECT TransactionID FROM FinancialTransactions WHERE StudentID = @StudentID AND TransactionType = 'Opening Balance'";
                            int txId = -1;
                            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn, trans))
                            {
                                checkCmd.Parameters.AddWithValue("@StudentID", id);
                                object res = checkCmd.ExecuteScalar();
                                if (res != null && res != DBNull.Value)
                                {
                                    txId = Convert.ToInt32(res);
                                }
                            }

                            if (request.OpeningBalanceAmount > 0)
                            {
                                decimal debit = 0;
                                decimal credit = 0;
                                if (!string.IsNullOrEmpty(request.BalanceType) && request.BalanceType.Equals("Debit", StringComparison.OrdinalIgnoreCase))
                                {
                                    debit = request.OpeningBalanceAmount;
                                }
                                else
                                {
                                    credit = request.OpeningBalanceAmount;
                                }

                                if (txId != -1)
                                {
                                    string updateTxQuery = "UPDATE FinancialTransactions SET Debit = @Debit, Credit = @Credit, Notes = @Notes WHERE TransactionID = @TransactionID";
                                    using (SqlCommand updateTxCmd = new SqlCommand(updateTxQuery, conn, trans))
                                    {
                                        updateTxCmd.Parameters.AddWithValue("@Debit", debit);
                                        updateTxCmd.Parameters.AddWithValue("@Credit", credit);
                                        updateTxCmd.Parameters.AddWithValue("@Notes", "رصيد افتتاح سابق");
                                        updateTxCmd.Parameters.AddWithValue("@TransactionID", txId);
                                        updateTxCmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    string insertTxQuery = @"
                                        INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID)
                                        VALUES (@StudentID, 'Opening Balance', @Debit, @Credit, @TransactionDate, @Notes, @UserID)";
                                    using (SqlCommand insertTxCmd = new SqlCommand(insertTxQuery, conn, trans))
                                    {
                                        insertTxCmd.Parameters.AddWithValue("@StudentID", id);
                                        insertTxCmd.Parameters.AddWithValue("@Debit", debit);
                                        insertTxCmd.Parameters.AddWithValue("@Credit", credit);
                                        insertTxCmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                                        insertTxCmd.Parameters.AddWithValue("@Notes", "رصيد افتتاح سابق");
                                        insertTxCmd.Parameters.AddWithValue("@UserID", 1);
                                        insertTxCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                if (txId != -1)
                                {
                                    string deleteTxQuery = "DELETE FROM FinancialTransactions WHERE TransactionID = @TransactionID";
                                    using (SqlCommand deleteTxCmd = new SqlCommand(deleteTxQuery, conn, trans))
                                    {
                                        deleteTxCmd.Parameters.AddWithValue("@TransactionID", txId);
                                        deleteTxCmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            trans.Commit();
                            return Ok(new { message = "تم تعديل بيانات الطالب والرصيد السابق بنجاح" });
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء تعديل بيانات الطالب ورصيده السابق", error = ex.Message });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ في السيرفر", error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteStudent(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Check if student exists
                    string checkQuery = "SELECT COUNT(*) FROM Students WHERE StudentID = @StudentID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@StudentID", id);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                        {
                            return Content(System.Net.HttpStatusCode.NotFound, new { message = "الطالب غير موجود" });
                        }
                    }

                    // Delete (Cascading deletes financial transactions due to ON DELETE CASCADE constraint on foreign keys)
                    string deleteQuery = "DELETE FROM Students WHERE StudentID = @StudentID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(new { message = "تم حذف الطالب وحركاته المالية بنجاح" });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء حذف الطالب", error = ex.Message });
            }
        }
    }

    public class StudentRequest
    {
        public string StudentName { get; set; }
        public string GuardianName { get; set; }
        public string ParentPhone { get; set; }
        public string Notes { get; set; }
        public decimal OpeningBalanceAmount { get; set; }
        public string BalanceType { get; set; }
    }

    public class StudentResponse
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string GuardianName { get; set; }
        public string ParentPhone { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Notes { get; set; }
        public decimal OpeningBalanceAmount { get; set; }
        public string BalanceType { get; set; }
    }
}
