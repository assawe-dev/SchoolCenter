Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows
Imports System.Windows.Controls

Public Class UsersView
    Private selectedUserID As Integer = 0

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        LoadUsersData()
        LoadAuditLogData()
        ResetForm()
    End Sub

    Private Sub LoadUsersData()
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT U.UserID, U.Username, U.PasswordHash, U.Role, U.IsActive, " &
                                     "P.CanManageStudents, P.CanManageCourses, P.CanAssignDues, P.CanReceivePayments, P.CanViewReports, P.CanManageUsers " &
                                     "FROM Users U " &
                                     "LEFT JOIN UserPermissions P ON U.UserID = P.UserID " &
                                     "ORDER BY U.UserID ASC"
                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.Fill(dt)
                End Using
            End Using

            dt.Columns.Add("StatusText", GetType(String))
            For Each row As DataRow In dt.Rows
                Dim active As Boolean = Convert.ToBoolean(row("IsActive"))
                row("StatusText") = If(active, "نشط 🟢", "معطل 🔴")
            Next

            dgUsers.ItemsSource = dt.DefaultView
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل المستخدمين: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub dgUsers_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If dgUsers.SelectedItem IsNot Nothing Then
            Dim row As DataRowView = CType(dgUsers.SelectedItem, DataRowView)
            selectedUserID = Convert.ToInt32(row("UserID"))
            txtUsername.Text = row("Username").ToString()
            txtPassword.Password = row("PasswordHash").ToString()

            Dim roleStr As String = row("Role").ToString()
            cmbRole.SelectedIndex = If(roleStr.ToLower().Contains("admin"), 0, 1)
            chkIsActive.IsChecked = Convert.ToBoolean(row("IsActive"))

            chkCanManageStudents.IsChecked = If(row("CanManageStudents") Is DBNull.Value, True, Convert.ToBoolean(row("CanManageStudents")))
            chkCanManageCourses.IsChecked = If(row("CanManageCourses") Is DBNull.Value, True, Convert.ToBoolean(row("CanManageCourses")))
            chkCanAssignDues.IsChecked = If(row("CanAssignDues") Is DBNull.Value, True, Convert.ToBoolean(row("CanAssignDues")))
            chkCanReceivePayments.IsChecked = If(row("CanReceivePayments") Is DBNull.Value, True, Convert.ToBoolean(row("CanReceivePayments")))
            chkCanViewReports.IsChecked = If(row("CanViewReports") Is DBNull.Value, False, Convert.ToBoolean(row("CanViewReports")))
            chkCanManageUsers.IsChecked = If(row("CanManageUsers") Is DBNull.Value, False, Convert.ToBoolean(row("CanManageUsers")))

            txtFormTitle.Text = "تعديل المستخدم (" & selectedUserID & ")"
            btnDeleteUser.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub btnSaveUser_Click(sender As Object, e As RoutedEventArgs)
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Password.Trim()
        Dim role As String = If(cmbRole.SelectedIndex = 0, "Admin", "Accountant")
        Dim isActive As Boolean = chkIsActive.IsChecked.GetValueOrDefault()

        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show("يرجى إدخال اسم المستخدم وكلمة المرور.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim trans As SqlTransaction = conn.BeginTransaction()

                Try
                    Dim targetUserID As Integer = selectedUserID

                    If selectedUserID = 0 Then
                        ' إضافة مستخدم جديد
                        Dim insertUserQuery As String = "INSERT INTO Users (Username, PasswordHash, Role, IsActive) VALUES (@Username, @Password, @Role, @IsActive); SELECT SCOPE_IDENTITY();"
                        Using cmd As New SqlCommand(insertUserQuery, conn, trans)
                            cmd.Parameters.AddWithValue("@Username", username)
                            cmd.Parameters.AddWithValue("@Password", password)
                            cmd.Parameters.AddWithValue("@Role", role)
                            cmd.Parameters.AddWithValue("@IsActive", isActive)
                            targetUserID = Convert.ToInt32(cmd.ExecuteScalar())
                        End Using
                    Else
                        ' تعديل مستخدم موجود
                        Dim updateUserQuery As String = "UPDATE Users SET Username = @Username, PasswordHash = @Password, Role = @Role, IsActive = @IsActive WHERE UserID = @UserID"
                        Using cmd As New SqlCommand(updateUserQuery, conn, trans)
                            cmd.Parameters.AddWithValue("@Username", username)
                            cmd.Parameters.AddWithValue("@Password", password)
                            cmd.Parameters.AddWithValue("@Role", role)
                            cmd.Parameters.AddWithValue("@IsActive", isActive)
                            cmd.Parameters.AddWithValue("@UserID", selectedUserID)
                            cmd.ExecuteNonQuery()
                        End Using
                    End If

                    ' حفظ/تحديث جدول الصلاحيات UserPermissions
                    Dim mergePermQuery As String = "IF EXISTS (SELECT * FROM UserPermissions WHERE UserID = @UserID) " &
                        "UPDATE UserPermissions SET CanManageStudents = @P1, CanManageCourses = @P2, CanAssignDues = @P3, CanReceivePayments = @P4, CanViewReports = @P5, CanManageUsers = @P6 WHERE UserID = @UserID " &
                        "ELSE " &
                        "INSERT INTO UserPermissions (UserID, CanManageStudents, CanManageCourses, CanAssignDues, CanReceivePayments, CanViewReports, CanManageUsers) VALUES (@UserID, @P1, @P2, @P3, @P4, @P5, @P6)"

                    Using cmd As New SqlCommand(mergePermQuery, conn, trans)
                        cmd.Parameters.AddWithValue("@UserID", targetUserID)
                        cmd.Parameters.AddWithValue("@P1", chkCanManageStudents.IsChecked.GetValueOrDefault())
                        cmd.Parameters.AddWithValue("@P2", chkCanManageCourses.IsChecked.GetValueOrDefault())
                        cmd.Parameters.AddWithValue("@P3", chkCanAssignDues.IsChecked.GetValueOrDefault())
                        cmd.Parameters.AddWithValue("@P4", chkCanReceivePayments.IsChecked.GetValueOrDefault())
                        cmd.Parameters.AddWithValue("@P5", chkCanViewReports.IsChecked.GetValueOrDefault())
                        cmd.Parameters.AddWithValue("@P6", chkCanManageUsers.IsChecked.GetValueOrDefault())
                        cmd.ExecuteNonQuery()
                    End Using

                    trans.Commit()
                    If selectedUserID = 0 Then
                        DbConnectionManager.LogAudit("إضافة مستخدم", "تم إنشاء حساب مستخدم جديد: " & username & " بدور: " & role)
                    Else
                        DbConnectionManager.LogAudit("تعديل مستخدم", "تم تحديث بيانات وملاءمة صلاحيات المستخدم رقم (" & selectedUserID & "): " & username)
                    End If
                    MessageBox.Show("تم حفظ بيانات المستخدم والصلاحيات بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
                    LoadUsersData()
                    ResetForm()
                Catch ex As Exception
                    trans.Rollback()
                    Throw ex
                End Try
            End Using
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء حفظ المستخدم: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnDeleteUser_Click(sender As Object, e As RoutedEventArgs)
        If selectedUserID = 0 Then Return

        If selectedUserID = UserSession.CurrentUserID Then
            MessageBox.Show("لا يمكنك حذف الحساب الخاص بك وأنت مسجل الدخول به حالياً.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        If MessageBox.Show("هل أنت متأكد من حذف هذا المستخدم؟", "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Warning) = MessageBoxResult.Yes Then
            Try
                Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                    conn.Open()
                    Dim query As String = "DELETE FROM Users WHERE UserID = @UserID"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@UserID", selectedUserID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                DbConnectionManager.LogAudit("حذف مستخدم", "تم حذف حساب المستخدم رقم (" & selectedUserID & ")")
                MessageBox.Show("تم حذف المستخدم بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
                LoadUsersData()
                ResetForm()
            Catch ex As Exception
                MessageBox.Show("حدث خطأ أثناء حذف المستخدم: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End If
    End Sub

    Private Sub LoadAuditLogData()
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT TOP 200 LogID, LogDate, ISNULL(Username, N'النظام') AS Username, ActionType, Description " &
                                     "FROM AuditLog ORDER BY LogID DESC"
                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.Fill(dt)
                End Using
            End Using

            dgAuditLog.ItemsSource = dt.DefaultView
        Catch ex As Exception
            ' Handling error
        End Try
    End Sub

    Private Sub btnRefreshAudit_Click(sender As Object, e As RoutedEventArgs)
        LoadAuditLogData()
    End Sub

    Private Sub btnPrintAudit_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim dv As DataView = CType(dgAuditLog.ItemsSource, DataView)
            If dv Is Nothing OrElse dv.Count = 0 Then Return
            Dim dt As DataTable = dv.ToTable()

            Dim cols As New Generic.List(Of ReportColumn)()
            cols.Add(New ReportColumn("رقم الحركة", "LogID", 0.8))
            cols.Add(New ReportColumn("التاريخ والوقت", "LogDate", 1.4))
            cols.Add(New ReportColumn("المستخدم", "Username", 1.2))
            cols.Add(New ReportColumn("نوع الإجراء", "ActionType", 1.2))
            cols.Add(New ReportColumn("تفاصيل الإجراء / التغييرات", "Description", 3.0))

            Dim doc As Documents.FlowDocument = PrintingService.CreateReportDocument("سجل تتبع ومراقبة حركات النظام (Audit Log)", Nothing, dt, cols, "التوثيق والتتبع الشامل للعمليات")
            PrintingService.PrintDocument(doc, "سجل المراقبة")
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء الطباعة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnExportAudit_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim dv As DataView = CType(dgAuditLog.ItemsSource, DataView)
            If dv Is Nothing OrElse dv.Count = 0 Then Return
            Dim dt As DataTable = dv.ToTable()
            PrintingService.ExportDataTableToCSV(dt, "سجل_مراقبة_النظام")
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء التصدير: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnAddNewUser_Click(sender As Object, e As RoutedEventArgs)
        ResetForm()
    End Sub

    Private Sub btnCancelEdit_Click(sender As Object, e As RoutedEventArgs)
        ResetForm()
    End Sub

    Private Sub ResetForm()
        selectedUserID = 0
        txtUsername.Text = ""
        txtPassword.Password = ""
        cmbRole.SelectedIndex = 1 ' Accountant by default
        chkIsActive.IsChecked = True

        chkCanManageStudents.IsChecked = True
        chkCanManageCourses.IsChecked = True
        chkCanAssignDues.IsChecked = True
        chkCanReceivePayments.IsChecked = True
        chkCanViewReports.IsChecked = False
        chkCanManageUsers.IsChecked = False

        txtFormTitle.Text = "إضافة مستخدم جديد"
        btnDeleteUser.Visibility = Visibility.Collapsed
        dgUsers.UnselectAll()
    End Sub
End Class
