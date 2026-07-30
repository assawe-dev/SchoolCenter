using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SchoolCenter.API.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private string GetConnectionString()
        {
            var connStrSetting = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (connStrSetting != null && !string.IsNullOrEmpty(connStrSetting.ConnectionString))
            {
                return connStrSetting.ConnectionString;
            }
            return "Server=.\\SQLEXPRESS;Database=SchoolCenterDB;Integrated Security=True;";
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetUsers(string search = null)
        {
            try
            {
                var list = new List<UserListResponse>();

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"
                        SELECT u.UserID, u.Username, u.Role, u.IsActive
                        FROM Users u";

                    if (!string.IsNullOrEmpty(search))
                    {
                        query += " WHERE u.Username LIKE @Filter OR u.Role LIKE @Filter";
                    }

                    query += " ORDER BY u.UserID DESC";

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
                                list.Add(new UserListResponse
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    Username = reader["Username"].ToString() ?? string.Empty,
                                    Role = reader["Role"].ToString() ?? string.Empty,
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                });
                            }
                        }
                    }
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء جلب قائمة المستخدمين", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetUserById")]
        public IHttpActionResult GetUser(int id)
        {
            try
            {
                UserResponse user = null;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"
                        SELECT u.UserID, u.Username, u.Role, u.IsActive,
                               p.CanManageStudents, p.CanManageCourses, p.CanAssignDues,
                               p.CanReceivePayments, p.CanViewReports, p.CanManageUsers
                        FROM Users u
                        LEFT JOIN UserPermissions p ON u.UserID = p.UserID
                        WHERE u.UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserResponse
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    Username = reader["Username"].ToString() ?? string.Empty,
                                    Role = reader["Role"].ToString() ?? string.Empty,
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    Permissions = new PermissionsModel
                                    {
                                        CanManageStudents = reader["CanManageStudents"] != DBNull.Value && Convert.ToBoolean(reader["CanManageStudents"]),
                                        CanManageCourses = reader["CanManageCourses"] != DBNull.Value && Convert.ToBoolean(reader["CanManageCourses"]),
                                        CanAssignDues = reader["CanAssignDues"] != DBNull.Value && Convert.ToBoolean(reader["CanAssignDues"]),
                                        CanReceivePayments = reader["CanReceivePayments"] != DBNull.Value && Convert.ToBoolean(reader["CanReceivePayments"]),
                                        CanViewReports = reader["CanViewReports"] != DBNull.Value && Convert.ToBoolean(reader["CanViewReports"]),
                                        CanManageUsers = reader["CanManageUsers"] != DBNull.Value && Convert.ToBoolean(reader["CanManageUsers"])
                                    }
                                };
                            }
                        }
                    }
                }

                if (user == null)
                {
                    return Content(System.Net.HttpStatusCode.NotFound, new { message = "المستخدم غير موجود" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء جلب بيانات المستخدم", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateUser([FromBody] CreateUserRequest request)
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

                    // Check username unique
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @user";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@user", request.Username.Trim());
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            return BadRequest("اسم المستخدم مسجل مسبقاً في النظام");
                        }
                    }

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            int newUserID = -1;
                            string queryUser = @"
                                INSERT INTO Users (Username, PasswordHash, Role, IsActive)
                                VALUES (@user, @pass, @role, @active);
                                SELECT SCOPE_IDENTITY();";

                            using (SqlCommand cmdUser = new SqlCommand(queryUser, conn, trans))
                            {
                                cmdUser.Parameters.AddWithValue("@user", request.Username.Trim());
                                cmdUser.Parameters.AddWithValue("@pass", request.Password.Trim());
                                cmdUser.Parameters.AddWithValue("@role", string.IsNullOrEmpty(request.Role) ? "Receptionist" : request.Role.Trim());
                                cmdUser.Parameters.AddWithValue("@active", request.IsActive ? 1 : 0);

                                object res = cmdUser.ExecuteScalar();
                                if (res != null && res != DBNull.Value)
                                {
                                    newUserID = Convert.ToInt32(res);
                                }
                            }

                            if (newUserID != -1)
                            {
                                string queryPerm = @"
                                    INSERT INTO UserPermissions (UserID, CanManageStudents, CanManageCourses, CanAssignDues, CanReceivePayments, CanViewReports, CanManageUsers)
                                    VALUES (@UserID, @CanStudents, @CanCourses, @CanDues, @CanPayments, @CanReports, @CanUsers)";

                                using (SqlCommand cmdPerm = new SqlCommand(queryPerm, conn, trans))
                                {
                                    var p = request.Permissions ?? new PermissionsModel();
                                    cmdPerm.Parameters.AddWithValue("@UserID", newUserID);
                                    cmdPerm.Parameters.AddWithValue("@CanStudents", p.CanManageStudents ? 1 : 0);
                                    cmdPerm.Parameters.AddWithValue("@CanCourses", p.CanManageCourses ? 1 : 0);
                                    cmdPerm.Parameters.AddWithValue("@CanDues", p.CanAssignDues ? 1 : 0);
                                    cmdPerm.Parameters.AddWithValue("@CanPayments", p.CanReceivePayments ? 1 : 0);
                                    cmdPerm.Parameters.AddWithValue("@CanReports", p.CanViewReports ? 1 : 0);
                                    cmdPerm.Parameters.AddWithValue("@CanUsers", p.CanManageUsers ? 1 : 0);

                                    cmdPerm.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                            return CreatedAtRoute("GetUserById", new { id = newUserID }, new { userID = newUserID, message = "تم إضافة المستخدم وصلاحياته بنجاح" });
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء حفظ المستخدم في السيرفر", error = ex.Message });
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
        public IHttpActionResult UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username))
            {
                return BadRequest("اسم المستخدم مطلوب");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Check user exists
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE UserID = @UserID";
                    using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", id);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                        {
                            return Content(System.Net.HttpStatusCode.NotFound, new { message = "المستخدم غير موجود" });
                        }
                    }

                    // Check username unique
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @user AND UserID <> @UserID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@user", request.Username.Trim());
                        checkCmd.Parameters.AddWithValue("@UserID", id);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            return BadRequest("اسم المستخدم مستخدم بالفعل من قبل حساب آخر");
                        }
                    }

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string queryUser;
                            bool updatePassword = !string.IsNullOrWhiteSpace(request.Password);

                            if (updatePassword)
                            {
                                queryUser = "UPDATE Users SET Username = @user, PasswordHash = @pass, Role = @role, IsActive = @active WHERE UserID = @UserID";
                            }
                            else
                            {
                                queryUser = "UPDATE Users SET Username = @user, Role = @role, IsActive = @active WHERE UserID = @UserID";
                            }

                            using (SqlCommand cmdUser = new SqlCommand(queryUser, conn, trans))
                            {
                                cmdUser.Parameters.AddWithValue("@user", request.Username.Trim());
                                if (updatePassword)
                                {
                                    cmdUser.Parameters.AddWithValue("@pass", request.Password.Trim());
                                }
                                cmdUser.Parameters.AddWithValue("@role", string.IsNullOrEmpty(request.Role) ? "Receptionist" : request.Role.Trim());
                                cmdUser.Parameters.AddWithValue("@active", request.IsActive ? 1 : 0);
                                cmdUser.Parameters.AddWithValue("@UserID", id);

                                cmdUser.ExecuteNonQuery();
                            }

                            // Update Permissions
                            string queryCheckPerm = "SELECT COUNT(*) FROM UserPermissions WHERE UserID = @UserID";
                            int permCount = 0;
                            using (SqlCommand cmdCheckPerm = new SqlCommand(queryCheckPerm, conn, trans))
                            {
                                cmdCheckPerm.Parameters.AddWithValue("@UserID", id);
                                permCount = Convert.ToInt32(cmdCheckPerm.ExecuteScalar());
                            }

                            var p = request.Permissions ?? new PermissionsModel();

                            if (permCount > 0)
                            {
                                string queryUpdatePerm = @"
                                    UPDATE UserPermissions
                                    SET CanManageStudents = @CanStudents,
                                        CanManageCourses = @CanCourses,
                                        CanAssignDues = @CanDues,
                                        CanReceivePayments = @CanPayments,
                                        CanViewReports = @CanReports,
                                        CanManageUsers = @CanUsers
                                    WHERE UserID = @UserID";

                                using (SqlCommand cmdUpdatePerm = new SqlCommand(queryUpdatePerm, conn, trans))
                                {
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanStudents", p.CanManageStudents ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanCourses", p.CanManageCourses ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanDues", p.CanAssignDues ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanPayments", p.CanReceivePayments ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanReports", p.CanViewReports ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanUsers", p.CanManageUsers ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@UserID", id);

                                    cmdUpdatePerm.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                string queryInsertPerm = @"
                                    INSERT INTO UserPermissions (UserID, CanManageStudents, CanManageCourses, CanAssignDues, CanReceivePayments, CanViewReports, CanManageUsers)
                                    VALUES (@UserID, @CanStudents, @CanCourses, @CanDues, @CanPayments, @CanReports, @CanUsers)";

                                using (SqlCommand cmdInsertPerm = new SqlCommand(queryInsertPerm, conn, trans))
                                {
                                    cmdInsertPerm.Parameters.AddWithValue("@UserID", id);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanStudents", p.CanManageStudents ? 1 : 0);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanCourses", p.CanManageCourses ? 1 : 0);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanDues", p.CanAssignDues ? 1 : 0);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanPayments", p.CanReceivePayments ? 1 : 0);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanReports", p.CanViewReports ? 1 : 0);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanUsers", p.CanManageUsers ? 1 : 0);

                                    cmdInsertPerm.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                            return Ok(new { message = "تم تعديل بيانات المستخدم وصلاحياته بنجاح" });
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء تعديل المستخدم في السيرفر", error = ex.Message });
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
        public IHttpActionResult DeleteUser(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Check exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE UserID = @UserID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", id);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                        {
                            return Content(System.Net.HttpStatusCode.NotFound, new { message = "المستخدم غير موجود" });
                        }
                    }

                    // Delete (permissions are cascade deleted via foreign key constraint)
                    string query = "DELETE FROM Users WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(new { message = "تم حذف المستخدم بنجاح" });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء حذف المستخدم", error = ex.Message });
            }
        }
    }

    public class UserListResponse
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserResponse
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public PermissionsModel Permissions { get; set; }
    }

    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public PermissionsModel Permissions { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; } // Optional
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public PermissionsModel Permissions { get; set; }
    }

    public class PermissionsModel
    {
        public bool CanManageStudents { get; set; }
        public bool CanManageCourses { get; set; }
        public bool CanAssignDues { get; set; }
        public bool CanReceivePayments { get; set; }
        public bool CanViewReports { get; set; }
        public bool CanManageUsers { get; set; }
    }
}
