using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SchoolCenter.API.Controllers
{
    [RoutePrefix("api/settings")]
    public class SettingsController : ApiController
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
        public IHttpActionResult GetSettings()
        {
            try
            {
                string centerName = "منظومة مركز الدورات التعليمية";
                string logoBase64 = null;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = "SELECT CenterName, LogoData FROM SystemSettings WHERE SettingID = 1";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                centerName = reader["CenterName"].ToString();
                                if (reader["LogoData"] != DBNull.Value)
                                {
                                    byte[] bytes = (byte[])reader["LogoData"];
                                    logoBase64 = Convert.ToBase64String(bytes);
                                }
                            }
                        }
                    }
                }

                return Ok(new SettingsResponse
                {
                    CenterName = centerName,
                    LogoBase64 = logoBase64
                });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء جلب إعدادات النظام", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult SaveSettings([FromBody] SettingsRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CenterName))
            {
                return BadRequest("يرجى إدخال اسم المركز التعليمي");
            }

            try
            {
                byte[] logoBytes = null;
                if (!string.IsNullOrEmpty(request.LogoBase64))
                {
                    try
                    {
                        logoBytes = Convert.FromBase64String(request.LogoBase64);
                    }
                    catch
                    {
                        return BadRequest("صيغة اللوجو المرسلة غير صالحة (Base64)");
                    }
                }

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"
                        UPDATE SystemSettings
                        SET CenterName = @CenterName, LogoData = @LogoData
                        WHERE SettingID = 1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CenterName", request.CenterName.Trim());
                        cmd.Parameters.AddWithValue("@LogoData", (object)logoBytes ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(new { message = "تم حفظ الإعدادات بنجاح" });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء حفظ الإعدادات في السيرفر", error = ex.Message });
            }
        }
    }

    public class SettingsRequest
    {
        public string CenterName { get; set; }
        public string LogoBase64 { get; set; }
    }

    public class SettingsResponse
    {
        public string CenterName { get; set; }
        public string LogoBase64 { get; set; }
    }
}
