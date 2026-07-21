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
        /// تهيئة قاعدة البيانات وإنشاء الجداول إذا لم تكن موجودة مع إدخال بيانات افتراضية للدورات
        /// </summary>
        public static void InitializeDatabase()
        {
            string connectionString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 1. إنشاء جدول الدورات
                string createCoursesTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses')
                    BEGIN
                        CREATE TABLE Courses (
                            CourseID INT IDENTITY(1,1) PRIMARY KEY,
                            CourseName NVARCHAR(100) NOT NULL,
                            CourseFees DECIMAL(18, 2) NOT NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createCoursesTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 2. إنشاء جدول الطلاب
                string createStudentsTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Students')
                    BEGIN
                        CREATE TABLE Students (
                            StudentID INT IDENTITY(1,1) PRIMARY KEY,
                            FullName NVARCHAR(100) NOT NULL,
                            Phone NVARCHAR(50) NOT NULL,
                            CourseID INT NOT NULL FOREIGN KEY REFERENCES Courses(CourseID),
                            RegistrationDate DATETIME NOT NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createStudentsTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 3. إنشاء جدول المعاملات المالية
                string createTransactionsTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
                    BEGIN
                        CREATE TABLE FinancialTransactions (
                            TransactionID INT IDENTITY(1,1) PRIMARY KEY,
                            StudentID INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID) ON DELETE CASCADE,
                            Amount DECIMAL(18, 2) NOT NULL,
                            TransactionDate DATETIME NOT NULL,
                            Notes NVARCHAR(250) NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createTransactionsTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 4. إدخال دورات افتراضية إذا كان الجدول فارغاً
                string seedCourses = @"
                    IF NOT EXISTS (SELECT * FROM Courses)
                    BEGIN
                        INSERT INTO Courses (CourseName, CourseFees) VALUES
                        (N'لغة إنجليزية - مستوى مبتدئ', 150.00),
                        (N'لغة إنجليزية - مستوى متوسط', 200.00),
                        (N'برمجة وتطوير تطبيقات سطح المكتب', 350.00),
                        (N'أساسيات شبكات الحاسوب', 250.00),
                        (N'التصميم الجرافيكي والملتيميديا', 300.00);
                    END";
                using (SqlCommand cmd = new SqlCommand(seedCourses, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

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