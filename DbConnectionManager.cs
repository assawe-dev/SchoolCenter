using System;
using System.IO;
using System.Data.SqlClient;

namespace SchoolCenter
{
    public static class DbConnectionManager
    {
        private static readonly string ConfigFileName = "db_config.txt";
        private static string _connectionString = null;

        /// <summary>
        /// جلب نص الاتصال بقاعدة البيانات من الملف الخارجي db_config.txt
        /// </summary>
        public static string GetConnectionString()
        {
            // إذا تم قراءة نص الاتصال مسبقاً، نقوم بإرجاعه مباشرة لتوفير الأداء
            if (!string.IsNullOrEmpty(_connectionString))
            {
                return _connectionString;
            }

            // تحديد مسار الملف الخارجي في مجلد Debug أو Release بجانب ملف الـ EXE
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);

            // إذا كان الملف غير موجود، نقوم بإنشائه تلقائياً بنموذج افتراضي
            if (!File.Exists(configPath))
            {
                CreateDefaultConfigFile(configPath);
                throw new FileNotFoundException("لم يتم العثور على ملف إعدادات قاعدة البيانات.\n" +
                    "تم إنشاء ملف افتراضي جديد باسم 'db_config.txt' في مجلد المنظومة (bin\\Debug).\n" +
                    "يرجى تعديل بيانات السيرفر داخل الملف ثم إعادة تشغيل المنظومة.");
            }

            try
            {
                var builder = new SqlConnectionStringBuilder();
                string[] lines = File.ReadAllLines(configPath);

                foreach (string line in lines)
                {
                    // تجاهل السطور الفارغة أو التعليقات التي تبدأ بـ #
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
                                {
                                    builder.IntegratedSecurity = integrated;
                                }
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

                builder.ConnectTimeout = 15; // وقت المحاولة قبل إظهار خطأ
                builder.Pooling = true;       // تسريع العمليات المتكررة

                _connectionString = builder.ConnectionString;
                return _connectionString;
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء قراءة ملف إعدادات قاعدة البيانات: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// إنشاء قالب ملف الإعدادات الافتراضي
        /// </summary>
        private static void CreateDefaultConfigFile(string path)
        {
            string defaultContent = "# =========================================================\n" +
                                    "# ملف إعدادات الاتصال بقاعدة البيانات - منظومة مركز الدورات\n" +
                                    "# =========================================================\n" +
                                    "SERVER=.\\SQLEXPRESS\n" +
                                    "DATABASE=SchoolCenterDB\n" +
                                    "INTEGRATED_SECURITY=True\n" +
                                    "USER ID=\n" +
                                    "PASSWORD=\n";
            File.WriteAllText(path, defaultContent, System.Text.Encoding.UTF8);
        }
    }
}