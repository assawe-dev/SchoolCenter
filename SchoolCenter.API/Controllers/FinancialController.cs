using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace SchoolCenter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FinancialController(IConfiguration configuration)
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

        [HttpPost("dues")]
        public IActionResult AssignDues([FromBody] DueAssignmentRequest request)
        {
            if (request == null || request.StudentID <= 0 || request.CourseID <= 0)
            {
                return BadRequest(new { message = "يرجى تحديد الطالب والدورة التدريبية" });
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Retrieve course details
                    decimal amount = request.CustomAmount;
                    string courseName = "";
                    string queryCourse = "SELECT CourseName, Cost FROM Courses WHERE CourseID = @CourseID";
                    using (SqlCommand cmdCourse = new SqlCommand(queryCourse, conn))
                    {
                        cmdCourse.Parameters.AddWithValue("@CourseID", request.CourseID);
                        using (SqlDataReader readerCourse = cmdCourse.ExecuteReader())
                        {
                            if (readerCourse.Read())
                            {
                                if (amount <= 0)
                                {
                                    amount = Convert.ToDecimal(readerCourse["Cost"]);
                                }
                                courseName = readerCourse["CourseName"].ToString() ?? "";
                            }
                            else
                            {
                                return NotFound(new { message = "الدورة التدريبية غير موجودة" });
                            }
                        }
                    }

                    // Check if student exists
                    string queryStudent = "SELECT COUNT(*) FROM Students WHERE StudentID = @StudentID";
                    using (SqlCommand cmdStudent = new SqlCommand(queryStudent, conn))
                    {
                        cmdStudent.Parameters.AddWithValue("@StudentID", request.StudentID);
                        if (Convert.ToInt32(cmdStudent.ExecuteScalar()) == 0)
                        {
                            return NotFound(new { message = "الطالب غير موجود" });
                        }
                    }

                    // Insert Fee Charge into FinancialTransactions
                    string queryInsert = @"
                        INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID)
                        VALUES (@StudentID, 'Fee Charge', @Debit, 0.00, @TransactionDate, @Notes, @UserID);
                        SELECT SCOPE_IDENTITY();";

                    int newTxID = -1;
                    using (SqlCommand cmdInsert = new SqlCommand(queryInsert, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@StudentID", request.StudentID);
                        cmdInsert.Parameters.AddWithValue("@Debit", amount);
                        cmdInsert.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                        cmdInsert.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(request.Notes) ? "تعيين دورة: " + courseName : request.Notes.Trim());
                        cmdInsert.Parameters.AddWithValue("@UserID", request.UserID <= 0 ? 1 : request.UserID);

                        object res = cmdInsert.ExecuteScalar();
                        if (res != null && res != DBNull.Value)
                        {
                            newTxID = Convert.ToInt32(res);
                        }
                    }

                    return Ok(new { transactionID = newTxID, amount, message = "تم تعيين المستحقات المالية بنجاح" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء تعيين المستحقات المالية", error = ex.Message });
            }
        }

        [HttpPost("payments")]
        public IActionResult ReceivePayment([FromBody] PaymentRequest request)
        {
            if (request == null || request.StudentID <= 0 || request.Amount <= 0)
            {
                return BadRequest(new { message = "يرجى تحديد الطالب ومبلغ سداد صحيح أكبر من الصفر" });
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
                        checkCmd.Parameters.AddWithValue("@StudentID", request.StudentID);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                        {
                            return NotFound(new { message = "الطالب غير موجود" });
                        }
                    }

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Insert into FinancialTransactions (Payment Receipt)
                            string queryTx = @"
                                INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID)
                                VALUES (@StudentID, 'Payment Receipt', 0.00, @Credit, @TransactionDate, @Notes, @UserID);
                                SELECT SCOPE_IDENTITY();";

                            int newTxID = -1;
                            using (SqlCommand cmdTx = new SqlCommand(queryTx, conn, trans))
                            {
                                cmdTx.Parameters.AddWithValue("@StudentID", request.StudentID);
                                cmdTx.Parameters.AddWithValue("@Credit", request.Amount);
                                cmdTx.Parameters.AddWithValue("@TransactionDate", request.PaymentDate);
                                cmdTx.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(request.Notes) ? "إيصال سداد رسوم" : request.Notes.Trim());
                                cmdTx.Parameters.AddWithValue("@UserID", request.UserID <= 0 ? 1 : request.UserID);

                                object res = cmdTx.ExecuteScalar();
                                if (res != null && res != DBNull.Value)
                                {
                                    newTxID = Convert.ToInt32(res);
                                }
                            }

                            if (newTxID != -1)
                            {
                                // Calculate current treasury balance in TreasuryLog
                                string queryBalance = @"
                                    SELECT ISNULL((SELECT SUM(Amount) FROM TreasuryLog WHERE ActionType = 'Deposit'), 0) -
                                           ISNULL((SELECT SUM(Amount) FROM TreasuryLog WHERE ActionType = 'Withdrawal'), 0)";
                                decimal currentBal = 0m;
                                using (SqlCommand cmdBal = new SqlCommand(queryBalance, conn, trans))
                                {
                                    object res = cmdBal.ExecuteScalar();
                                    if (res != null && res != DBNull.Value)
                                    {
                                        currentBal = Convert.ToDecimal(res);
                                    }
                                }

                                decimal newBalance = currentBal + request.Amount;

                                // 2. Insert deposit entry into TreasuryLog
                                string queryLog = @"
                                    INSERT INTO TreasuryLog (TransactionID, Amount, ActionType, CurrentBalance, LogDate, Notes)
                                    VALUES (@TransactionID, @Amount, 'Deposit', @CurrentBalance, @LogDate, @Notes)";
                                using (SqlCommand cmdLog = new SqlCommand(queryLog, conn, trans))
                                {
                                    cmdLog.Parameters.AddWithValue("@TransactionID", newTxID);
                                    cmdLog.Parameters.AddWithValue("@Amount", request.Amount);
                                    cmdLog.Parameters.AddWithValue("@CurrentBalance", newBalance);
                                    cmdLog.Parameters.AddWithValue("@LogDate", request.PaymentDate);
                                    cmdLog.Parameters.AddWithValue("@Notes", "إيداع تلقائي لقيمة إيصال رقم " + newTxID);
                                    cmdLog.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                            return Ok(new { transactionID = newTxID, message = "تم تسجيل الإيصال المالي وتحديث الخزينة بنجاح" });
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return StatusCode(500, new { message = "حدث خطأ أثناء معالجة السداد وتحديث الخزينة في السيرفر", error = ex.Message });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ في السيرفر", error = ex.Message });
            }
        }

        [HttpGet("statement")]
        public IActionResult GetAccountStatement([FromQuery] int studentId, [FromQuery] string? fromDate, [FromQuery] string? toDate, [FromQuery] string? export)
        {
            if (studentId <= 0)
            {
                return BadRequest(new { message = "يرجى تحديد معرف الطالب" });
            }

            try
            {
                DateTime parsedFrom = DateTime.MinValue;
                DateTime parsedTo = DateTime.MaxValue;

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime.TryParse(fromDate, out parsedFrom);
                }
                else
                {
                    // Default to current month start
                    parsedFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime.TryParse(toDate, out parsedTo);
                }
                parsedTo = parsedTo.Date.AddDays(1).AddSeconds(-1); // include the whole selected end date

                var statementRows = new List<StatementRow>();
                string studentName = "";
                decimal runningBalance = 0m;
                decimal totalCharged = 0m;
                decimal totalPaid = 0m;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Get Student Name
                    string nameQuery = "SELECT StudentName FROM Students WHERE StudentID = @StudentID";
                    using (SqlCommand nameCmd = new SqlCommand(nameQuery, conn))
                    {
                        nameCmd.Parameters.AddWithValue("@StudentID", studentId);
                        object res = nameCmd.ExecuteScalar();
                        if (res != null && res != DBNull.Value)
                        {
                            studentName = res.ToString() ?? "";
                        }
                        else
                        {
                            return NotFound(new { message = "الطالب غير موجود" });
                        }
                    }

                    // Get All Transactions chronologically
                    string query = @"
                        SELECT
                            ft.TransactionDate,
                            ft.TransactionType,
                            ft.Notes,
                            ft.Debit,
                            ft.Credit,
                            u.Username
                        FROM FinancialTransactions ft
                        INNER JOIN Users u ON ft.UserID = u.UserID
                        WHERE ft.StudentID = @StudentID
                          AND ft.TransactionDate >= @FromDate
                          AND ft.TransactionDate <= @ToDate
                        ORDER BY ft.TransactionDate ASC, ft.TransactionID ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        cmd.Parameters.AddWithValue("@FromDate", parsedFrom);
                        cmd.Parameters.AddWithValue("@ToDate", parsedTo);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime date = Convert.ToDateTime(reader["TransactionDate"]);
                                string type = reader["TransactionType"].ToString() ?? "";
                                string notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() ?? "" : "";
                                decimal debit = Convert.ToDecimal(reader["Debit"]);
                                decimal credit = Convert.ToDecimal(reader["Credit"]);
                                string username = reader["Username"] != DBNull.Value ? reader["Username"].ToString() ?? "-" : "-";

                                runningBalance += (debit - credit);
                                totalCharged += debit;
                                totalPaid += credit;

                                string arabicType = type;
                                if (type == "Fee Charge") arabicType = "رسوم دورة";
                                else if (type == "Payment Receipt") arabicType = "سند قبض";
                                else if (type == "Opening Balance") arabicType = "رصيد سابق";

                                statementRows.Add(new StatementRow
                                {
                                    TransactionDate = date,
                                    TransactionType = type,
                                    ArabicType = arabicType,
                                    Notes = notes,
                                    Debit = debit,
                                    Credit = credit,
                                    RunningBalance = runningBalance,
                                    HandlingEmployee = username
                                });
                            }
                        }
                    }
                }

                // Support simple zero-dependency CSV format export
                if (!string.IsNullOrEmpty(export) && export.Equals("csv", StringComparison.OrdinalIgnoreCase))
                {
                    StringBuilder sb = new StringBuilder();

                    // Standard Arabic RTL Headers
                    // Write UTF-8 BOM first to ensure proper Excel loading for Arabic text
                    sb.Append((char)0xFEFF); // BOM character

                    sb.AppendLine("تاريخ الحركة,نوع الحركة,البيان / الملاحظات,المطلوب / مدين,المدفوع / دائن,الرصيد المتبقي التراكمي,الموظف المسؤول");

                    foreach (var row in statementRows)
                    {
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                            EscapeCsvField(row.TransactionDate.ToString("yyyy/MM/dd HH:mm")),
                            EscapeCsvField(row.ArabicType),
                            EscapeCsvField(row.Notes),
                            row.Debit.ToString("N2"),
                            row.Credit.ToString("N2"),
                            row.RunningBalance.ToString("N2"),
                            EscapeCsvField(row.HandlingEmployee)
                        ));
                    }

                    sb.AppendLine();
                    sb.AppendLine(string.Format("{0},,,,,{1}", EscapeCsvField("إجمالي المطلوب"), totalCharged.ToString("N2") + " د.ل"));
                    sb.AppendLine(string.Format("{0},,,,,{1}", EscapeCsvField("إجمالي المدفوع"), totalPaid.ToString("N2") + " د.ل"));
                    sb.AppendLine(string.Format("{0},,,,,{1}", EscapeCsvField("الرصيد المتبقي النهائي"), (totalCharged - totalPaid).ToString("N2") + " د.ل"));

                    byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
                    return File(data, "text/csv; charset=utf-8", $"statement_student_{studentId}.csv");
                }

                return Ok(new
                {
                    studentID = studentId,
                    studentName,
                    fromDate = parsedFrom,
                    toDate = parsedTo,
                    totalCharged,
                    totalPaid,
                    finalBalance = totalCharged - totalPaid,
                    transactions = statementRows
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء توليد كشف الحساب للطالب", error = ex.Message });
            }
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }
            return field;
        }
    }

    public class DueAssignmentRequest
    {
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public decimal CustomAmount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int UserID { get; set; } = 1;
    }

    public class PaymentRequest
    {
        public int StudentID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
        public int UserID { get; set; } = 1;
    }

    public class StatementRow
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string ArabicType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal RunningBalance { get; set; }
        public string HandlingEmployee { get; set; } = string.Empty;
    }
}
