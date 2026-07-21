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

                // التحقق من وجود الهيكل القديم وحذفه لتجنب تعارضات الأسماء والأعمدة الجديدة
                string dropOldSchema = @"
                    -- إذا كان جدول الطلاب يحتوي على العمود القديم Phone أو الهيكل القديم
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Students') AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Students') AND name = 'Phone')
                    BEGIN
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
                            DROP TABLE FinancialTransactions;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentDues')
                            DROP TABLE StudentDues;
                        DROP TABLE Students;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses')
                            DROP TABLE Courses;
                    END

                    -- إذا كان جدول الدورات يحتوي على العمود القديم CourseID
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses') AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Courses') AND name = 'CourseID')
                    BEGIN
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
                            DROP TABLE FinancialTransactions;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentDues')
                            DROP TABLE StudentDues;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Students')
                            DROP TABLE Students;
                        DROP TABLE Courses;
                    END";

                using (SqlCommand cmd = new SqlCommand(dropOldSchema, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 1. إنشاء جدول الدورات الجديد
                string createCoursesTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses')
                    BEGIN
                        CREATE TABLE Courses (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            CourseName NVARCHAR(100) NOT NULL,
                            Cost DECIMAL(18, 2) NOT NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createCoursesTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 2. إنشاء جدول الطلاب الجديد
                string createStudentsTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Students')
                    BEGIN
                        CREATE TABLE Students (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            FullName NVARCHAR(100) NOT NULL,
                            GuardianName NVARCHAR(100) NOT NULL,
                            GuardianPhone NVARCHAR(50) NOT NULL,
                            Notes NVARCHAR(250) NULL,
                            CreatedAt DATETIME NOT NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createStudentsTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 3. إنشاء جدول المستحقات المالية الجديد StudentDues
                string createStudentDuesTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentDues')
                    BEGIN
                        CREATE TABLE StudentDues (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            StudentId INT NOT NULL FOREIGN KEY REFERENCES Students(Id) ON DELETE CASCADE,
                            CourseId INT NOT NULL FOREIGN KEY REFERENCES Courses(Id) ON DELETE CASCADE,
                            DueAmount DECIMAL(18, 2) NOT NULL,
                            Notes NVARCHAR(250) NULL,
                            CreatedAt DATETIME NOT NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createStudentDuesTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 4. إنشاء جدول الحركات المالية المعدل
                string createTransactionsTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
                    BEGIN
                        CREATE TABLE FinancialTransactions (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            StudentId INT NOT NULL FOREIGN KEY REFERENCES Students(Id) ON DELETE CASCADE,
                            Amount DECIMAL(18, 2) NOT NULL,
                            TransactionDate DATETIME NOT NULL,
                            Notes NVARCHAR(250) NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createTransactionsTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 5. إدخال دورات افتراضية إذا كان جدول الدورات فارغاً
                string seedCourses = @"
                    IF NOT EXISTS (SELECT * FROM Courses)
                    BEGIN
                        INSERT INTO Courses (CourseName, Cost) VALUES
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
            if (!string.IsNullOrEmpty(_connectionString))
            {
                return _connectionString;
            }

            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);

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

                builder.ConnectTimeout = 15;
                builder.Pooling = true;

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