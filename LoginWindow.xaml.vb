Imports System
Imports System.Data.SqlClient
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input

Public Class LoginWindow
    Private _lastFocusedControl As Control

    Private Sub InputControl_GotFocus(sender As Object, e As RoutedEventArgs)
        _lastFocusedControl = TryCast(sender, Control)
    End Sub

    Private Sub BtnLogin_Click(sender As Object, e As RoutedEventArgs)
        Dim username As String = TxtUsername.Text.Trim()
        Dim password As String = TxtPassword.Password.Trim()

        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show("يرجى إدخال اسم المستخدم وكلمة المرور.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
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
                                MessageBox.Show("هذا الحساب معطل حالياً. يرجى مراجعة مدير النظام.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
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

                            DbConnectionManager.LogAudit("تسجيل الدخول", "تم تسجيل الدخول بنجاح بواسطة المستخدم: " & UserSession.Username & " (" & UserSession.Role & ")")

                            ' الانتقال إلى الشاشة الرئيسية
                            Dim mainWin As New MainWindow()
                            mainWin.Show()
                            Me.Close()
                        Else
                            MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة.", "خطأ في تسجيل الدخول", MessageBoxButton.OK, MessageBoxImage.Error)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("تعذر الاتصال بقاعدة البيانات: " & ex.Message, "خطأ اتصال", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub BtnExit_Click(sender As Object, e As RoutedEventArgs)
        Application.Current.Shutdown()
    End Sub

    Private Sub Numpad_Click(sender As Object, e As RoutedEventArgs)
        Dim btn As Button = TryCast(sender, Button)
        If btn Is Nothing OrElse btn.Content Is Nothing Then Return

        Dim digit As String = btn.Content.ToString()

        If _lastFocusedControl Is TxtUsername OrElse TxtUsername.IsFocused Then
            TxtUsername.Text &= digit
            TxtUsername.CaretIndex = TxtUsername.Text.Length
            TxtUsername.Focus()
        Else
            TxtPassword.Password &= digit
            TxtPassword.Focus()
        End If
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As RoutedEventArgs)
        If _lastFocusedControl Is TxtUsername OrElse TxtUsername.IsFocused Then
            TxtUsername.Clear()
            TxtUsername.Focus()
        ElseIf _lastFocusedControl Is TxtPassword OrElse TxtPassword.IsFocused Then
            TxtPassword.Clear()
            TxtPassword.Focus()
        Else
            TxtPassword.Clear()
            TxtUsername.Clear()
        End If
    End Sub
End Class
