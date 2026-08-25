Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows
Imports System.Windows.Controls

Public Class StudentDuesView
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        LoadStudentsCombo()
        LoadCoursesCombo()
        LoadRecentDues()
    End Sub

    Private Sub LoadStudentsCombo()
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT StudentID, StudentName + ' (' + ParentPhone + ')' AS DisplayName FROM Students ORDER BY StudentName ASC"
                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.Fill(dt)
                End Using
            End Using

            cmbStudents.ItemsSource = dt.DefaultView
        Catch ex As Exception
            ' Handling error
        End Try
    End Sub

    Private Sub LoadCoursesCombo()
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT CourseID, CourseName, Cost FROM Courses ORDER BY CourseName ASC"
                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.Fill(dt)
                End Using
            End Using

            cmbCourses.ItemsSource = dt.DefaultView
        Catch ex As Exception
            ' Handling error
        End Try
    End Sub

    Private Sub cmbStudents_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If cmbStudents.SelectedValue IsNot Nothing Then
            Dim studentId As Integer = Convert.ToInt32(cmbStudents.SelectedValue)
            Dim balance As Decimal = FinancialService.GetStudentBalance(studentId)
            txtCurrentStudentDebt.Text = balance.ToString("N2") & " د.ل"
            pnlStudentDebtBanner.Visibility = Visibility.Visible
        Else
            pnlStudentDebtBanner.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub cmbCourses_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If cmbCourses.SelectedItem IsNot Nothing Then
            Dim row As DataRowView = CType(cmbCourses.SelectedItem, DataRowView)
            Dim cost As Decimal = Convert.ToDecimal(row("Cost"))
            txtAmount.Text = cost.ToString("F2")

            If String.IsNullOrEmpty(txtNotes.Text.Trim()) Then
                txtNotes.Text = "رسوم " & row("CourseName").ToString()
            End If
        End If
    End Sub

    Private Sub btnSaveDue_Click(sender As Object, e As RoutedEventArgs)
        If cmbStudents.SelectedValue Is Nothing Then
            MessageBox.Show("يرجى اختيار الطالب من القائمة.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        Dim studentId As Integer = Convert.ToInt32(cmbStudents.SelectedValue)
        Dim amount As Decimal = 0
        If Not Decimal.TryParse(txtAmount.Text.Trim(), amount) OrElse amount <= 0 Then
            MessageBox.Show("يرجى إدخال مبلغ مستحق صحيح أكبر من صفر.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        Dim notes As String = txtNotes.Text.Trim()

        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID) " &
                                     "VALUES (@StudentID, 'Fee Charge', @Debit, 0, GETDATE(), @Notes, @UserID)"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@StudentID", studentId)
                    cmd.Parameters.AddWithValue("@Debit", amount)
                    cmd.Parameters.AddWithValue("@Notes", If(String.IsNullOrEmpty(notes), DBNull.Value, CType(notes, Object)))
                    cmd.Parameters.AddWithValue("@UserID", UserSession.CurrentUserID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            Dim studentText As String = cmbStudents.Text
            DbConnectionManager.LogAudit("تسجيل في كورس", "تم تسجيل الطالب (" & studentText & ") بمبلغ مستحق: " & amount.ToString("N2") & " د.ل - ملاحظات: " & notes)
            MessageBox.Show("تم تسجيل الطالب في الكورس بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
            txtNotes.Text = ""
            txtAmount.Text = "0.00"
            cmbStudents_SelectionChanged(Nothing, Nothing)
            LoadRecentDues()
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء حفظ المستحق: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim dv As DataView = CType(dgDues.ItemsSource, DataView)
            If dv Is Nothing OrElse dv.Count = 0 Then Return
            Dim dt As DataTable = dv.ToTable()

            Dim cols As New Generic.List(Of ReportColumn)()
            cols.Add(New ReportColumn("رقم الحركة", "TransactionID", 0.8))
            cols.Add(New ReportColumn("اسم الطالب", "StudentName", 2.0))
            cols.Add(New ReportColumn("المبلغ المستحق", "Debit", 1.2))
            cols.Add(New ReportColumn("البيان", "Notes", 2.0))
            cols.Add(New ReportColumn("تاريخ التسجيل", "TransactionDate", 1.2))
            cols.Add(New ReportColumn("الموظف", "Username", 1.0))

            Dim doc As Documents.FlowDocument = PrintingService.CreateReportDocument("سجل التسجيلات والمستحقات", Nothing, dt, cols, "حركات الرسوم والمستحقات المسجلة مؤخراً")
            PrintingService.PrintDocument(doc, "سجل المستحقات")
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء الطباعة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnExport_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim dv As DataView = CType(dgDues.ItemsSource, DataView)
            If dv Is Nothing OrElse dv.Count = 0 Then Return
            Dim dt As DataTable = dv.ToTable()
            PrintingService.ExportDataTableToCSV(dt, "سجل_المستحقات")
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء التصدير: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub LoadRecentDues()
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT TOP 15 T.TransactionID, S.StudentName, T.Debit, T.Notes, T.TransactionDate, U.Username " &
                                     "FROM FinancialTransactions T " &
                                     "INNER JOIN Students S ON T.StudentID = S.StudentID " &
                                     "INNER JOIN Users U ON T.UserID = U.UserID " &
                                     "WHERE T.TransactionType = 'Fee Charge' " &
                                     "ORDER BY T.TransactionID DESC"
                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.Fill(dt)
                End Using
            End Using

            dgDues.ItemsSource = dt.DefaultView
        Catch ex As Exception
            ' Handling error
        End Try
    End Sub
End Class
