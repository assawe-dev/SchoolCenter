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
        /// تهيئة قاعدة البيانات وإنشاء الجداول إذا لم تكن موجودة مع إدخال بيانات افتراضية
        /// </summary>
        public static void InitializeDatabase()
        {
            string connectionString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // التحقق من وجود الهيكل القديم وحذفه لتجنب تعارضات الأسماء والأعمدة الجديدة
                string dropOldSchema = @"
                    -- حذف جدول StudentDues القديم غير المستخدم في الهيكل الجديد
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentDues')
                    BEGIN
                        DROP TABLE StudentDues;
                    END

                    -- إذا كان جدول الطلاب يحتوي على عمود Id بدلاً من StudentID
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Students') AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Students') AND name = 'Id')
                    BEGIN
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TreasuryLog')
                            DROP TABLE TreasuryLog;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
                            DROP TABLE FinancialTransactions;
                        DROP TABLE Students;
                    END

                    -- إذا كان جدول الدورات يحتوي على عمود Id بدلاً من CourseID
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses') AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Courses') AND name = 'Id')
                    BEGIN
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TreasuryLog')
                            DROP TABLE TreasuryLog;
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
                            DROP TABLE FinancialTransactions;
                        DROP TABLE Courses;
                    END

                    -- إذا كان جدول المعاملات لا يحتوي على عمود TransactionType
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions') AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('FinancialTransactions') AND name = 'TransactionType')
                    BEGIN
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TreasuryLog')
                            DROP TABLE TreasuryLog;
                        DROP TABLE FinancialTransactions;
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
                            Username NVARCHAR(100) NOT NULL,
                            PasswordHash NVARCHAR(100) NOT NULL,
                            Role NVARCHAR(50) NOT NULL,
                            IsActive BIT NOT NULL
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
                            RegistrationDate DATETIME NOT NULL,
                            Notes NVARCHAR(250) NULL
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
                            TransactionType NVARCHAR(50) NOT NULL, -- 'Fee Charge' / 'Payment Receipt'
                            Debit DECIMAL(18, 2) NOT NULL,
                            Credit DECIMAL(18, 2) NOT NULL,
                            TransactionDate DATETIME NOT NULL,
                            Notes NVARCHAR(250) NULL,
                            UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID)
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
                            ActionType NVARCHAR(50) NOT NULL, -- 'Deposit' / 'Withdrawal'
                            CurrentBalance DECIMAL(18, 2) NOT NULL,
                            LogDate DATETIME NOT NULL,
                            Notes NVARCHAR(250) NULL
                        );
                    END";
                using (SqlCommand cmd = new SqlCommand(createTreasuryLogTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 6. إدخال مستخدم افتراضي إذا كان جدول المستخدمين فارغاً
                string seedUsers = @"
                    IF NOT EXISTS (SELECT * FROM Users)
                    BEGIN
                        INSERT INTO Users (Username, PasswordHash, Role, IsActive) VALUES
                        (N'admin', N'admin123', N'Administrator', 1);
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