Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input

Public Class StudentsView
    Private selectedStudentID As Integer = 0

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        LoadStudentsData()
        ResetForm()
    End Sub

    Private Sub LoadStudentsData(Optional filter As String = "")
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT S.StudentID, S.StudentName, S.GuardianName, S.ParentPhone, S.RegistrationDate, S.Notes, " &
                                     "ISNULL(SUM(T.Debit), 0) - ISNULL(SUM(T.Credit), 0) AS CurrentBalance " &
                                     "FROM Students S " &
                                     "LEFT JOIN FinancialTransactions T ON S.StudentID = T.StudentID " &
                                     "WHERE 1=1 "

                If Not String.IsNullOrWhiteSpace(filter) Then
                    query &= "AND (S.StudentName LIKE @Filter OR S.GuardianName LIKE @Filter OR S.ParentPhone LIKE @Filter) "
                End If

                query &= "GROUP BY S.StudentID, S.StudentName, S.GuardianName, S.ParentPhone, S.RegistrationDate, S.Notes " &
                         "ORDER BY S.StudentID DESC"

                Using cmd As New SqlCommand(query, conn)
                    If Not String.IsNullOrWhiteSpace(filter) Then
                        cmd.Parameters.AddWithValue("@Filter", "%" & filter.Trim() & "%")
                    End If
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            dgStudents.ItemsSource = dt.DefaultView
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل بيانات الطلاب: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As TextChangedEventArgs)
        LoadStudentsData(txtSearch.Text)
    End Sub

    Private Sub dgStudents_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If dgStudents.SelectedItem IsNot Nothing Then
            Dim row As DataRowView = CType(dgStudents.SelectedItem, DataRowView)
            selectedStudentID = Convert.ToInt32(row("StudentID"))
            txtStudentName.Text = row("StudentName").ToString()
            txtParentPhone.Text = row("ParentPhone").ToString()
            txtNotes.Text = If(row("Notes") Is DBNull.Value, "", row("Notes").ToString())

            txtFormTitle.Text = "تعديل بيانات الطالب (" & selectedStudentID & ")"
            pnlOpeningBalance.Visibility = Visibility.Collapsed
            btnDeleteStudent.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub btnAddNewStudent_Click(sender As Object, e As RoutedEventArgs)
        ResetForm()
    End Sub

    Private Sub dgStudents_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        If dgStudents.SelectedItem IsNot Nothing Then
            Dim row As DataRowView = CType(dgStudents.SelectedItem, DataRowView)
            Dim studentId As Integer = Convert.ToInt32(row("StudentID"))
            Dim studentName As String = row("StudentName").ToString()

            Dim statementWin As New AccountStatementWindow(studentId, studentName)
            statementWin.Owner = Window.GetWindow(CType(Me, DependencyObject))
            statementWin.ShowDialog()
        End If
    End Sub

    Private Sub btnSaveStudent_Click(sender As Object, e As RoutedEventArgs)
        Dim name As String = txtStudentName.Text.Trim()
        Dim phone As String = txtParentPhone.Text.Trim()
        Dim notes As String = txtNotes.Text.Trim()

        If String.IsNullOrEmpty(name) OrElse String.IsNullOrEmpty(phone) Then
            MessageBox.Show("يرجى تعبئة كافة الحقول المطلوبة (*).", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()

                If selectedStudentID = 0 Then
                    ' إضافة طالب جديد
                    Dim openingBalance As Decimal = 0
                    Decimal.TryParse(txtOpeningBalanceAmount.Text.Trim(), openingBalance)

                    Dim trans As SqlTransaction = conn.BeginTransaction()
                    Try
                        ' 1. إدراج بيانات الطالب
                        Dim insertStudentQuery As String = "INSERT INTO Students (StudentName, GuardianName, ParentPhone, RegistrationDate, Notes) " &
                                                          "VALUES (@StudentName, @GuardianName, @ParentPhone, GETDATE(), @Notes); " &
                                                          "SELECT SCOPE_IDENTITY();"
                        Dim newStudentID As Integer = 0
                        Using cmd As New SqlCommand(insertStudentQuery, conn, trans)
                            cmd.Parameters.AddWithValue("@StudentName", name)
                            cmd.Parameters.AddWithValue("@GuardianName", name)
                            cmd.Parameters.AddWithValue("@ParentPhone", phone)
                            cmd.Parameters.AddWithValue("@Notes", If(String.IsNullOrEmpty(notes), DBNull.Value, CType(notes, Object)))
                            newStudentID = Convert.ToInt32(cmd.ExecuteScalar())
                        End Using

                        ' 2. إضافة الرصيد الافتتاحي إذا كان أكبر من صفر
                        If openingBalance > 0 Then
                            Dim isDebit As Boolean = (cmbBalanceType.SelectedIndex = 0)
                            Dim debitVal As Decimal = If(isDebit, openingBalance, 0)
                            Dim creditVal As Decimal = If(Not isDebit, openingBalance, 0)

                            Dim insertTransQuery As String = "INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID) " &
                                                            "VALUES (@StudentID, 'Opening Balance', @Debit, @Credit, GETDATE(), N'رصيد افتتاحي سابق', @UserID)"
                            Using cmd As New SqlCommand(insertTransQuery, conn, trans)
                                cmd.Parameters.AddWithValue("@StudentID", newStudentID)
                                cmd.Parameters.AddWithValue("@Debit", debitVal)
                                cmd.Parameters.AddWithValue("@Credit", creditVal)
                                cmd.Parameters.AddWithValue("@UserID", UserSession.CurrentUserID)
                                cmd.ExecuteNonQuery()
                            End Using
                        End If

                        trans.Commit()
                        DbConnectionManager.LogAudit("إضافة طالب", "تم تسجيل طالب جديد: " & name & " - رقم الهاتف: " & phone)
                        MessageBox.Show("تم إضافة الطالب بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
                    Catch ex As Exception
                        trans.Rollback()
                        Throw ex
                    End Try
                Else
                    ' تعديل بيانات طالب موجود
                    Dim updateQuery As String = "UPDATE Students SET StudentName = @StudentName, GuardianName = @GuardianName, ParentPhone = @ParentPhone, Notes = @Notes " &
                                               "WHERE StudentID = @StudentID"
                    Using cmd As New SqlCommand(updateQuery, conn)
                        cmd.Parameters.AddWithValue("@StudentName", name)
                        cmd.Parameters.AddWithValue("@GuardianName", name)
                        cmd.Parameters.AddWithValue("@ParentPhone", phone)
                        cmd.Parameters.AddWithValue("@Notes", If(String.IsNullOrEmpty(notes), DBNull.Value, CType(notes, Object)))
                        cmd.Parameters.AddWithValue("@StudentID", selectedStudentID)
                        cmd.ExecuteNonQuery()
                    End Using
                    DbConnectionManager.LogAudit("تعديل طالب", "تم تحديث بيانات الطالب رقم (" & selectedStudentID & "): " & name)
                    MessageBox.Show("تم تحديث بيانات الطالب بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            End Using

            LoadStudentsData()
            ResetForm()
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء حفظ بيانات الطالب: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnDeleteStudent_Click(sender As Object, e As RoutedEventArgs)
        If selectedStudentID = 0 Then Return

        If MessageBox.Show("هل أنت متاكد من حذف هذا الطالب وجميع حركاته المالية؟", "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Warning) = MessageBoxResult.Yes Then
            Try
                Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                    conn.Open()
                    Dim query As String = "DELETE FROM Students WHERE StudentID = @StudentID"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@StudentID", selectedStudentID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                DbConnectionManager.LogAudit("حذف طالب", "تم حذف الطالب رقم (" & selectedStudentID & ") وجميع حركاته المالية")
                MessageBox.Show("تم حذف الطالب بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
                LoadStudentsData()
                ResetForm()
            Catch ex As Exception
                MessageBox.Show("حدث خطأ أثناء حذف الطالب: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End If
    End Sub

    Private Sub btnCancelEdit_Click(sender As Object, e As RoutedEventArgs)
        ResetForm()
    End Sub

    Private Sub ResetForm()
        selectedStudentID = 0
        txtStudentName.Text = ""
        txtParentPhone.Text = ""
        txtNotes.Text = ""
        txtOpeningBalanceAmount.Text = "0.00"
        cmbBalanceType.SelectedIndex = 0

        txtFormTitle.Text = "إضافة طالب جديد"
        pnlOpeningBalance.Visibility = Visibility.Visible
        btnDeleteStudent.Visibility = Visibility.Collapsed
        dgStudents.UnselectAll()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim dv As DataView = CType(dgStudents.ItemsSource, DataView)
            If dv Is Nothing OrElse dv.Count = 0 Then Return
            Dim dt As DataTable = dv.ToTable()

            Dim cols As New Generic.List(Of ReportColumn)()
            cols.Add(New ReportColumn("رقم الطالب", "StudentID", 0.8))
            cols.Add(New ReportColumn("اسم الطالب بالكامل", "StudentName", 2.0))
            cols.Add(New ReportColumn("رقم الهاتف", "ParentPhone", 1.2))
            cols.Add(New ReportColumn("الرصيد المتبقي", "CurrentBalance", 1.2))
            cols.Add(New ReportColumn("تاريخ التسجيل", "RegistrationDate", 1.2))

            Dim doc As Documents.FlowDocument = PrintingService.CreateReportDocument("قائمة الطلاب المسجلين", Nothing, dt, cols, "سجل بيانات الطلاب والديون المتبقية")
            PrintingService.PrintDocument(doc, "قائمة الطلاب")
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء الطباعة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnExport_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim dv As DataView = CType(dgStudents.ItemsSource, DataView)
            If dv Is Nothing OrElse dv.Count = 0 Then Return
            Dim dt As DataTable = dv.ToTable()
            PrintingService.ExportDataTableToCSV(dt, "قائمة_الطلاب")
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء التصدير: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnShowStatement_Click(sender As Object, e As RoutedEventArgs)
        If selectedStudentID = 0 Then
            MessageBox.Show("يرجى تحديد طالب أولاً من القائمة لعرض كشف الحساب.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Information)
            Return
        End If

        Dim statementWin As New AccountStatementWindow(selectedStudentID, txtStudentName.Text)
        statementWin.Owner = Window.GetWindow(CType(Me, DependencyObject))
        statementWin.ShowDialog()
    End Sub
End Class
