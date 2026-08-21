Imports System
Imports System.Data.SqlClient
Imports System.Windows
Imports System.Windows.Input

Public Class LoginWindow
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ' تحديث اسم المركز في الشاشة من الإعدادات
        Try
            Dim settings As SettingsService.CenterSettings = SettingsService.GetSettings()
            If Not String.IsNullOrEmpty(settings.CenterName) Then
                txtBrandTitle.Text = settings.CenterName
            End If
        Catch ex As Exception
        End Try
        txtUsername.Focus()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As RoutedEventArgs)
        pnlError.Visibility = Visibility.Collapsed
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Password.Trim()

        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            ShowError("يرجى إدخال اسم المستخدم وكلمة المرور.")
            Return
        End If

        Try
            Dim connStr As String = DbConnectionManager.GetConnectionString()
            Using conn As New SqlConnection(connStr)
                conn.Open()
                Dim query As String = "SELECT U.UserID, U.Username, U.Role, U.IsActive, " &
                                     "P.CanManageStudents, P.CanManageCourses, P.CanAssignDues, P.CanReceivePayments, P.CanViewReports, P.CanManageUsers " &
                                     "FROM Users U " &
                                     "LEFT JOIN UserPermissions P ON U.UserID = P.UserID " &
                                     "WHERE U.Username = @Username AND U.PasswordHash = @Password"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Username", username)
                    cmd.Parameters.AddWithValue("@Password", password)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim isActive As Boolean = Convert.ToBoolean(reader("IsActive"))
                            If Not isActive Then
                                ShowError("هذا الحساب معطل حالياً. يرجى مراجعة مدير النظام.")
                                Return
                            End If

                            ' تسجيل بيانات الجلسة
                            UserSession.CurrentUserID = Convert.ToInt32(reader("UserID"))
                            UserSession.Username = reader("Username").ToString()
                            UserSession.Role = reader("Role").ToString()

                            ' الصلاحيات
                            UserSession.CanManageStudents = If(reader("CanManageStudents") Is DBNull.Value, True, Convert.ToBoolean(reader("CanManageStudents")))
                            UserSession.CanManageCourses = If(reader("CanManageCourses") Is DBNull.Value, True, Convert.ToBoolean(reader("CanManageCourses")))
                            UserSession.CanAssignDues = If(reader("CanAssignDues") Is DBNull.Value, True, Convert.ToBoolean(reader("CanAssignDues")))
                            UserSession.CanReceivePayments = If(reader("CanReceivePayments") Is DBNull.Value, True, Convert.ToBoolean(reader("CanReceivePayments")))
                            UserSession.CanViewReports = If(reader("CanViewReports") Is DBNull.Value, True, Convert.ToBoolean(reader("CanViewReports")))
                            UserSession.CanManageUsers = If(reader("CanManageUsers") Is DBNull.Value, True, Convert.ToBoolean(reader("CanManageUsers")))

                            ' الانتقال إلى الشاشة الرئيسية
                            Dim mainWin As New MainWindow()
                            mainWin.Show()
                            Me.Close()
                        Else
                            ShowError("اسم المستخدم أو كلمة المرور غير صحيحة.")
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ShowError("تعذر الاتصال بقاعدة البيانات: " & ex.Message)
        End Try
    End Sub

    Private Sub ShowError(message As String)
        txtErrorMessage.Text = message
        pnlError.Visibility = Visibility.Visible
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            btnLogin_Click(sender, e)
        End If
    End Sub
End Class
