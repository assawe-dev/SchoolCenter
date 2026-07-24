using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

namespace SchoolCenter
{
    public static class SettingsService
    {
        public static Tuple<string, Image> GetSettings()
        {
            string centerName = "منظومة مركز الدورات التعليمية";
            Image logo = null;

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
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
                                    using (MemoryStream ms = new MemoryStream(bytes))
                                    {
                                        logo = new Bitmap(ms);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetSettings failed: " + ex.Message);
            }

            return new Tuple<string, Image>(centerName, logo);
        }

        public static void SaveSettings(string centerName, Image logo)
        {
            try
            {
                byte[] logoBytes = null;
                if (logo != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Save image to stream as PNG to preserve transparency
                        logo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        logoBytes = ms.ToArray();
                    }
                }

                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        UPDATE SystemSettings
                        SET CenterName = @CenterName, LogoData = @LogoData
                        WHERE SettingID = 1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CenterName", centerName);
                        cmd.Parameters.AddWithValue("@LogoData", (object)logoBytes ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء حفظ الإعدادات: " + ex.Message, ex);
            }
        }
    }
}
