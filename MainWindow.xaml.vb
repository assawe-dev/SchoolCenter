Imports System
Imports System.Windows
Imports System.Windows.Controls

Public Class MainWindow
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ' إعداد معلومات المستخدم والجلسة
        txtCurrentUsername.Text = UserSession.Username
        txtCurrentUserRole.Text = UserSession.Role

        ' فحص الصلاحيات للتحكم بظهور الأزرار
        ApplyUserPermissions()

        ' تحميل بيانات المركز
        RefreshCenterBranding()

        ' فتح الشاشة الرئيسية افتراضياً
        NavigateTo("Dashboard")
    End Sub

    Public Sub RefreshCenterBranding()
        Try
            Dim settings = SettingsService.GetSettings()
            If Not String.IsNullOrEmpty(settings.CenterName) Then
                txtCenterHeaderName.Text = settings.CenterName
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ApplyUserPermissions()
        btnStudents.Visibility = If(UserSession.CanManageStudents, Visibility.Visible, Visibility.Collapsed)
        btnCourses.Visibility = If(UserSession.CanManageCourses, Visibility.Visible, Visibility.Collapsed)
        btnDues.Visibility = If(UserSession.CanAssignDues, Visibility.Visible, Visibility.Collapsed)
        btnPayments.Visibility = If(UserSession.CanReceivePayments, Visibility.Visible, Visibility.Collapsed)
        btnReports.Visibility = If(UserSession.CanViewReports, Visibility.Visible, Visibility.Collapsed)
        btnUsers.Visibility = If(UserSession.CanManageUsers, Visibility.Visible, Visibility.Collapsed)
        btnSettings.Visibility = If(UserSession.CanManageUsers, Visibility.Visible, Visibility.Collapsed)
    End Sub

    Private Sub NavButton_Click(sender As Object, e As RoutedEventArgs)
        Dim btn As Button = TryCast(sender, Button)
        If btn Is Nothing Then Return

        ' إلغاء تحديد جميع الأزرار
        For Each child In pnlNavButtons.Children
            If TypeOf child Is Button Then
                CType(child, Button).Tag = Nothing
            End If
        Next

        btn.Tag = "Active"

        If btn Is btnDashboard Then
            NavigateTo("Dashboard")
        ElseIf btn Is btnStudents Then
            NavigateTo("Students")
        ElseIf btn Is btnCourses Then
            NavigateTo("Courses")
        ElseIf btn Is btnDues Then
            NavigateTo("StudentDues")
        ElseIf btn Is btnPayments Then
            NavigateTo("Payments")
        ElseIf btn Is btnReports Then
            NavigateTo("BalanceReport")
        ElseIf btn Is btnUsers Then
            NavigateTo("Users")
        ElseIf btn Is btnSettings Then
            NavigateTo("Settings")
        End If
    End Sub

    Public Sub NavigateTo(viewName As String)
        Select Case viewName
            Case "Dashboard"
                txtHeaderTitle.Text = "لوحة التحكم الإحصائية"
                mainContentControl.Content = New DashboardView()
            Case "Students"
                txtHeaderTitle.Text = "إدارة سجل الطلاب"
                mainContentControl.Content = New StudentsView()
            Case "Courses"
                txtHeaderTitle.Text = "إدارة سجل الدورات التعليمية"
                mainContentControl.Content = New CoursesView()
            Case "StudentDues"
                txtHeaderTitle.Text = "تسجيل المستحقات والديون"
                mainContentControl.Content = New StudentDuesView()
            Case "Payments"
                txtHeaderTitle.Text = "سندات القبض وحركة الخزينة"
                mainContentControl.Content = New PaymentsView()
            Case "BalanceReport"
                txtHeaderTitle.Text = "تقرير أرصدة الطلاب والديون"
                mainContentControl.Content = New BalanceReportView()
            Case "Users"
                txtHeaderTitle.Text = "إدارة حسابات المستخدمين والصلاحيات"
                mainContentControl.Content = New UsersView()
            Case "Settings"
                txtHeaderTitle.Text = "إعدادات النظام والمركز"
                mainContentControl.Content = New SettingsView()
        End Select
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As RoutedEventArgs)
        If MessageBox.Show("هل أنت تأكد من تسجيل الخروج من المنظومة؟", "تأكيد الخروج", MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes Then
            UserSession.ClearSession()
            Dim loginWin As New LoginWindow()
            loginWin.Show()
            Me.Close()
        End If
    End Sub
End Class
