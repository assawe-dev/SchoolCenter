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
        /// تهيئة قاعدة البيانات وإنشاء الجداول إذا لم تكن موجودة مع إدخال بيانات افتراضية للمستخدمين والدورات
        /// </summary>
        public static void InitializeDatabase()
        {
            string connectionString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // التحقق من وجود الهيكل القديم وحذفه لتجنب تعارضات الأسماء والأعمدة الجديدة
                string dropOldSchema = @"
                    -- إذا كان جدول الطلاب يحتوي على العمود القديم FullName أو الهيكل القديم
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Students') AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Students') AND name = 'FullName')
                       OR EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentDues')
                       OR EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions') AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('FinancialTransactions') AND name = 'Amount')
                       OR EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses') AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Courses') AND name = 'CourseID')
                    BEGIN
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TreasuryLog')
                            DROP TABLE TreasuryLog;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
                            DROP TABLE FinancialTransactions;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentDues')
                            DROP TABLE StudentDues;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Students')
                            DROP TABLE Students;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses')
                            DROP TABLE Courses;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                            DROP TABLE Users;
                    END";

                using (SqlCommand cmd = new SqlCommand(dropOldSchema, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 1. إنشاء جدول المستخدمين Users
                string createUsersTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                    BEGIN
                        CREATE TABLE Users (
                            UserID INT IDENTITY(1,1) PRIMARY KEY,
                            Username NVARCHAR(50) NOT NULL UNIQUE,
                            PasswordHash NVARCHAR(100) NOT NULL,
                            Role NVARCHAR(50) NOT NULL,
                            IsActive BIT NOT NULL DEFAULT 1
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createUsersTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 2. إنشاء جدول الطلاب Students
                string createStudentsTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Students')
                    BEGIN
                        CREATE TABLE Students (
                            StudentID INT IDENTITY(1,1) PRIMARY KEY,
                            StudentName NVARCHAR(100) NOT NULL,
                            GuardianName NVARCHAR(100) NOT NULL,
                            ParentPhone NVARCHAR(50) NOT NULL,
                            RegistrationDate DATETIME NOT NULL DEFAULT GETDATE(),
                            Notes NVARCHAR(500) NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createStudentsTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 3. إنشاء جدول الدورات Courses
                string createCoursesTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses')
                    BEGIN
                        CREATE TABLE Courses (
                            CourseID INT IDENTITY(1,1) PRIMARY KEY,
                            CourseName NVARCHAR(100) NOT NULL,
                            Cost DECIMAL(18, 2) NOT NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createCoursesTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 4. إنشاء جدول الحركات المالية FinancialTransactions
                string createTransactionsTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
                    BEGIN
                        CREATE TABLE FinancialTransactions (
                            TransactionID INT IDENTITY(1,1) PRIMARY KEY,
                            StudentID INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID) ON DELETE CASCADE,
                            TransactionType NVARCHAR(50) NOT NULL, -- 'Fee Charge' or 'Payment Receipt'
                            Debit DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
                            Credit DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
                            TransactionDate DATETIME NOT NULL DEFAULT GETDATE(),
                            Notes NVARCHAR(500) NULL,
                            UserID INT NULL FOREIGN KEY REFERENCES Users(UserID) ON DELETE SET NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createTransactionsTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 5. إنشاء جدول سجل الخزينة TreasuryLog
                string createTreasuryLogTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TreasuryLog')
                    BEGIN
                        CREATE TABLE TreasuryLog (
                            LogID INT IDENTITY(1,1) PRIMARY KEY,
                            TransactionID INT NOT NULL FOREIGN KEY REFERENCES FinancialTransactions(TransactionID) ON DELETE CASCADE,
                            Amount DECIMAL(18, 2) NOT NULL,
                            ActionType NVARCHAR(50) NOT NULL, -- 'Deposit' or 'Withdrawal'
                            CurrentBalance DECIMAL(18, 2) NOT NULL,
                            LogDate DATETIME NOT NULL DEFAULT GETDATE(),
                            Notes NVARCHAR(500) NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createTreasuryLogTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 6. إدخال مستخدم افتراضي إذا كان الجدول فارغاً
                string seedUsers = @"
                    IF NOT EXISTS (SELECT * FROM Users)
                    BEGIN
                        INSERT INTO Users (Username, PasswordHash, Role, IsActive)
                        VALUES (N'admin', N'admin123', N'Admin', 1);
                    END";
                using (SqlCommand cmd = new SqlCommand(seedUsers, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 7. إدخال دورات افتراضية إذا كان جدول الدورات فارغاً
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
        /// الحصول على أول معرف مستخدم نشط (لتسهيل المعاملات بدون شاشة تسجيل دخول كاملة)
        /// </summary>
        public static int GetDefaultUserID()
        {
            string connectionString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 1 UserID FROM Users WHERE IsActive = 1 ORDER BY UserID ASC";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            return 1; // Fallback
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