using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace SchoolCenter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connStr))
            {
                // Fallback to local config file or default
                return "Server=.\\SQLEXPRESS;Database=SchoolCenterDB;Integrated Security=True;TrustServerCertificate=True";
            }
            return connStr;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "يرجى إدخال اسم المستخدم وكلمة المرور" });
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"
                        SELECT u.UserID, u.Username, u.Role, u.IsActive,
                               p.CanManageStudents, p.CanManageCourses, p.CanAssignDues,
                               p.CanReceivePayments, p.CanViewReports, p.CanManageUsers
                        FROM Users u
                        LEFT JOIN UserPermissions p ON u.UserID = p.UserID
                        WHERE u.Username = @user AND u.PasswordHash = @pass AND u.IsActive = 1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", request.Username.Trim());
                        cmd.Parameters.AddWithValue("@pass", request.Password.Trim());

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var response = new LoginResponse
                                {
                                    Token = "mock-jwt-token-for-school-center-api-" + Guid.NewGuid().ToString("N"),
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    Username = reader["Username"].ToString() ?? string.Empty,
                                    Role = reader["Role"].ToString() ?? string.Empty,
                                    Permissions = new PermissionsResponse
                                    {
                                        CanManageStudents = reader["CanManageStudents"] != DBNull.Value && Convert.ToBoolean(reader["CanManageStudents"]),
                                        CanManageCourses = reader["CanManageCourses"] != DBNull.Value && Convert.ToBoolean(reader["CanManageCourses"]),
                                        CanAssignDues = reader["CanAssignDues"] != DBNull.Value && Convert.ToBoolean(reader["CanAssignDues"]),
                                        CanReceivePayments = reader["CanReceivePayments"] != DBNull.Value && Convert.ToBoolean(reader["CanReceivePayments"]),
                                        CanViewReports = reader["CanViewReports"] != DBNull.Value && Convert.ToBoolean(reader["CanViewReports"]),
                                        CanManageUsers = reader["CanManageUsers"] != DBNull.Value && Convert.ToBoolean(reader["CanManageUsers"])
                                    }
                                };
                                return Ok(response);
                            }
                        }
                    }
                }

                return Unauthorized(new { message = "اسم المستخدم أو كلمة المرور غير صحيحة" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "حدث خطأ في السيرفر أثناء محاولة تسجيل الدخول", error = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public PermissionsResponse Permissions { get; set; } = new();
    }

    public class PermissionsResponse
    {
        public bool CanManageStudents { get; set; }
        public bool CanManageCourses { get; set; }
        public bool CanAssignDues { get; set; }
        public bool CanReceivePayments { get; set; }
        public bool CanViewReports { get; set; }
        public bool CanManageUsers { get; set; }
    }
}
