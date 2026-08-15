Imports System
Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration

Public Class DbConnectionManager
    Private Shared ReadOnly ConfigFileName As String = "db_config.txt"
    Private Shared _connectionString As String = Nothing

    ''' <summary>
    ''' جلب نص الاتصال بقاعدة البيانات
    ''' </summary>
    Public Shared Function GetConnectionString() As String
        If Not String.IsNullOrEmpty(_connectionString) Then
            Return _connectionString
        End If

        ' تجربة جلب الاتصال من App.config أولاً
        Try
            If ConfigurationManager.ConnectionStrings("DefaultConnection") IsNot Nothing Then
                Dim configConnStr As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
                If Not String.IsNullOrWhiteSpace(configConnStr) Then
                    _connectionString = configConnStr
                    Return _connectionString
                End If
            End If
        Catch ex As Exception
            ' تجاهل والاستمرار مع ملف db_config.txt
        End Try

        Dim configPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName)

        If Not File.Exists(configPath) Then
            CreateDefaultConfigFile(configPath)
        End If

        Try
            Dim builder As New SqlConnectionStringBuilder()
            Dim lines As String() = File.ReadAllLines(configPath)

            For Each line As String In lines
                If String.IsNullOrWhiteSpace(line) OrElse line.Trim().StartsWith("#") Then Continue For

                Dim delimiterIndex As Integer = line.IndexOf("="c)
                If delimiterIndex > 0 Then
                    Dim key As String = line.Substring(0, delimiterIndex).Trim().ToUpper()
                    Dim value As String = line.Substring(delimiterIndex + 1).Trim()

                    Select Case key
                        Case "SERVER", "DATA SOURCE"
                            builder.DataSource = value
                        Case "DATABASE", "INITIAL CATALOG"
                            builder.InitialCatalog = value
                        Case "INTEGRATED_SECURITY", "INTEGRATED SECURITY"
                            Dim integrated As Boolean
                            If Boolean.TryParse(value, integrated) Then
                                builder.IntegratedSecurity = integrated
                            End If
                        Case "USER ID"
                            builder.UserID = value
                        Case "PASSWORD"
                            builder.Password = value
                    End Select
                End If
            Next

            builder.ConnectTimeout = 15
            builder.Pooling = True

            _connectionString = builder.ConnectionString
            Return _connectionString
        Catch ex As Exception
            ' العودة لنص افتراضي عند حدوث أي خطأ
            _connectionString = "Server=.\SQLEXPRESS;Database=SchoolCenterDB;Integrated Security=True;Connect Timeout=15;"
            Return _connectionString
        End Try
    End Function

    ''' <summary>
    ''' تهيئة قاعدة البيانات وإنشاء الجداول وتحديث الهيكل إن لزم الأمر
    ''' </summary>
    Public Shared Sub InitializeDatabase()
        Dim connStr As String = GetConnectionString()
        Using connection As New SqlConnection(connStr)
            connection.Open()

            ' 1. جدول المستخدمين Users
            Dim createUsersTable As String = "
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                BEGIN
                    CREATE TABLE Users (
                        UserID INT IDENTITY(1,1) PRIMARY KEY,
                        Username NVARCHAR(100) NOT NULL,
                        PasswordHash NVARCHAR(100) NOT NULL,
                        Role NVARCHAR(50) NOT NULL,
                        IsActive BIT NOT NULL
                    );
                END"
            Using cmd As New SqlCommand(createUsersTable, connection)
                cmd.ExecuteNonQuery()
            End Using

            ' 2. جدول الطلاب Students
            Dim createStudentsTable As String = "
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
                END"
            Using cmd As New SqlCommand(createStudentsTable, connection)
                cmd.ExecuteNonQuery()
            End Using

            ' 3. جدول الدورات Courses
            Dim createCoursesTable As String = "
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses')
                BEGIN
                    CREATE TABLE Courses (
                        CourseID INT IDENTITY(1,1) PRIMARY KEY,
                        CourseName NVARCHAR(100) NOT NULL,
                        Cost DECIMAL(18, 2) NOT NULL
                    );
                END"
            Using cmd As New SqlCommand(createCoursesTable, connection)
                cmd.ExecuteNonQuery()
            End Using

            ' 4. جدول الحركات المالية FinancialTransactions
            Dim createTransactionsTable As String = "
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
                BEGIN
                    CREATE TABLE FinancialTransactions (
                        TransactionID INT IDENTITY(1,1) PRIMARY KEY,
                        StudentID INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID) ON DELETE CASCADE,
                        TransactionType NVARCHAR(50) NOT NULL,
                        Debit DECIMAL(18, 2) NOT NULL,
                        Credit DECIMAL(18, 2) NOT NULL,
                        TransactionDate DATETIME NOT NULL,
                        Notes NVARCHAR(250) NULL,
                        UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID)
                    );
                END"
            Using cmd As New SqlCommand(createTransactionsTable, connection)
                cmd.ExecuteNonQuery()
            End Using

            ' 5. جدول سجل الخزينة TreasuryLog
            Dim createTreasuryLogTable As String = "
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TreasuryLog')
                BEGIN
                    CREATE TABLE TreasuryLog (
                        LogID INT IDENTITY(1,1) PRIMARY KEY,
                        TransactionID INT NOT NULL FOREIGN KEY REFERENCES FinancialTransactions(TransactionID) ON DELETE CASCADE,
                        Amount DECIMAL(18, 2) NOT NULL,
                        ActionType NVARCHAR(50) NOT NULL,
                        CurrentBalance DECIMAL(18, 2) NOT NULL,
                        LogDate DATETIME NOT NULL,
                        Notes NVARCHAR(250) NULL
                    );
                END"
            Using cmd As New SqlCommand(createTreasuryLogTable, connection)
                cmd.ExecuteNonQuery()
            End Using

            ' 6. جدول صلاحيات المستخدمين UserPermissions
            Dim createUserPermissionsTable As String = "
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserPermissions')
                BEGIN
                    CREATE TABLE UserPermissions (
                        PermissionID INT IDENTITY(1,1) PRIMARY KEY,
                        UserID INT NOT NULL,
                        CanManageStudents BIT DEFAULT 1 NOT NULL,
                        CanManageCourses BIT DEFAULT 1 NOT NULL,
                        CanAssignDues BIT DEFAULT 1 NOT NULL,
                        CanReceivePayments BIT DEFAULT 1 NOT NULL,
                        CanViewReports BIT DEFAULT 1 NOT NULL,
                        CanManageUsers BIT DEFAULT 1 NOT NULL,
                        CONSTRAINT FK_Permissions_Users FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
                    );
                END"
            Using cmd As New SqlCommand(createUserPermissionsTable, connection)
                cmd.ExecuteNonQuery()
            End Using

            ' 7. جدول إعدادات النظام SystemSettings
            Dim createSystemSettingsTable As String = "
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SystemSettings')
                BEGIN
                    CREATE TABLE SystemSettings (
                        SettingID INT PRIMARY KEY DEFAULT 1,
                        CenterName NVARCHAR(200) NOT NULL,
                        LogoData VARBINARY(MAX) NULL
                    );
                END"
            Using cmd As New SqlCommand(createSystemSettingsTable, connection)
                cmd.ExecuteNonQuery()
            End Using

            ' إدخال الإعدادات الافتراضية
            Dim seedSettings As String = "
                IF NOT EXISTS (SELECT * FROM SystemSettings)
                BEGIN
                    INSERT INTO SystemSettings (SettingID, CenterName, LogoData)
                    VALUES (1, N'منظومة مركز الدورات التعليمية', NULL);
                END"
            Using cmd As New SqlCommand(seedSettings, connection)
                cmd.ExecuteNonQuery()
            End Using

            ' إدخال مستخدم المسؤول الافتراضي
            Dim seedUsers As String = "
                IF NOT EXISTS (SELECT * FROM Users)
                BEGIN
                    DECLARE @NewUserID INT;
                    INSERT INTO Users (Username, PasswordHash, Role, IsActive) VALUES
                    (N'admin', N'admin123', N'Admin', 1);
                    SET @NewUserID = SCOPE_IDENTITY();

                    INSERT INTO UserPermissions (UserID, CanManageStudents, CanManageCourses, CanAssignDues, CanReceivePayments, CanViewReports, CanManageUsers)
                    VALUES (@NewUserID, 1, 1, 1, 1, 1, 1);
                END"
            Using cmd As New SqlCommand(seedUsers, connection)
                cmd.ExecuteNonQuery()
            End Using

            ' إدخال دورات تعليمية نموذجية
            Dim seedCourses As String = "
                IF NOT EXISTS (SELECT * FROM Courses)
                BEGIN
                    INSERT INTO Courses (CourseName, Cost) VALUES
                    (N'لغة إنجليزية - مستوى مبتدئ', 150.00),
                    (N'لغة إنجليزية - مستوى متوسط', 200.00),
                    (N'برمجة وتطوير تطبيقات سطح المكتب', 350.00),
                    (N'أساسيات شبكات الحاسوب', 250.00),
                    (N'التصميم الجرافيكي والملتيميديا', 300.00);
                END"
            Using cmd As New SqlCommand(seedCourses, connection)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Shared Sub CreateDefaultConfigFile(path As String)
        Dim defaultContent As String = "# =========================================================" & vbCrLf &
                                "# ملف إعدادات الاتصال بقاعدة البيانات - منظومة مركز الدورات" & vbCrLf &
                                "# =========================================================" & vbCrLf &
                                "SERVER=.\SQLEXPRESS" & vbCrLf &
                                "DATABASE=SchoolCenterDB" & vbCrLf &
                                "INTEGRATED_SECURITY=True" & vbCrLf &
                                "USER ID=" & vbCrLf &
                                "PASSWORD=" & vbCrLf
        File.WriteAllText(path, defaultContent, System.Text.Encoding.UTF8)
    End Sub
End Class
