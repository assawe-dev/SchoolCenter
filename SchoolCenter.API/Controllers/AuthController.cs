using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SchoolCenter.API.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
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

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("يرجى إدخال اسم المستخدم وكلمة المرور");
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

                return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "اسم المستخدم أو كلمة المرور غير صحيحة" });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ في السيرفر أثناء محاولة تسجيل الدخول", error = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public PermissionsResponse Permissions { get; set; }
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
