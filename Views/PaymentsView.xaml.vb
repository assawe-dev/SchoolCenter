Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows
Imports System.Windows.Controls

Public Class PaymentsView
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        LoadStudentsCombo()
        LoadRecentPayments()
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

    Private Sub cmbStudents_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If cmbStudents.SelectedValue IsNot Nothing Then
            Dim studentId As Integer = Convert.ToInt32(cmbStudents.SelectedValue)
            Dim debt As Decimal = FinancialService.GetStudentBalance(studentId)
            txtStudentDebt.Text = debt.ToString("N2") & " د.ل"
            pnlStudentDebtBanner.Visibility = Visibility.Visible
        Else
            pnlStudentDebtBanner.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub btnSavePayment_Click(sender As Object, e As RoutedEventArgs)
        If cmbStudents.SelectedValue Is Nothing Then
            MessageBox.Show("يرجى اختيار الطالب المسدد أولاً.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        Dim studentId As Integer = Convert.ToInt32(cmbStudents.SelectedValue)
        Dim amount As Decimal = 0
        If Not Decimal.TryParse(txtPaidAmount.Text.Trim(), amount) OrElse amount <= 0 Then
            MessageBox.Show("يرجى إدخال مبلغ مقبوض صحيح أكبر من صفر.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        Dim notes As String = txtNotes.Text.Trim()

        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim trans As SqlTransaction = conn.BeginTransaction()

                Try
                    ' 1. تسجيل الحركة المالية في FinancialTransactions
                    Dim transQuery As String = "INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID) " &
                                              "VALUES (@StudentID, 'Payment Receipt', 0, @Credit, GETDATE(), @Notes, @UserID); " &
                                              "SELECT SCOPE_IDENTITY();"
                    Dim transID As Integer = 0
                    Using cmd As New SqlCommand(transQuery, conn, trans)
                        cmd.Parameters.AddWithValue("@StudentID", studentId)
                        cmd.Parameters.AddWithValue("@Credit", amount)
                        cmd.Parameters.AddWithValue("@Notes", If(String.IsNullOrEmpty(notes), DBNull.Value, CType(notes, Object)))
                        cmd.Parameters.AddWithValue("@UserID", UserSession.CurrentUserID)
                        transID = Convert.ToInt32(cmd.ExecuteScalar())
                    End Using

                    ' 2. حساب إجمالي رصيد الخزينة بعد الإيداع
                    Dim newTreasuryBal As Decimal = 0
                    Dim sumQuery As String = "SELECT ISNULL(SUM(Credit), 0) FROM FinancialTransactions WHERE TransactionType = 'Payment Receipt'"
                    Using cmd As New SqlCommand(sumQuery, conn, trans)
                        newTreasuryBal = Convert.ToDecimal(cmd.ExecuteScalar())
                    End Using

                    ' 3. إضافة سجل بالخزينة TreasuryLog
                    Dim logQuery As String = "INSERT INTO TreasuryLog (TransactionID, Amount, ActionType, CurrentBalance, LogDate, Notes) " &
                                            "VALUES (@TransactionID, @Amount, 'Deposit', @CurrentBalance, GETDATE(), @Notes)"
                    Using cmd As New SqlCommand(logQuery, conn, trans)
                        cmd.Parameters.AddWithValue("@TransactionID", transID)
                        cmd.Parameters.AddWithValue("@Amount", amount)
                        cmd.Parameters.AddWithValue("@CurrentBalance", newTreasuryBal)
                        cmd.Parameters.AddWithValue("@Notes", If(String.IsNullOrEmpty(notes), DBNull.Value, CType(notes, Object)))
                        cmd.ExecuteNonQuery()
                    End Using

                    trans.Commit()
                    MessageBox.Show("تم تسليم سند القبض وإيداع المبلغ في الخزينة بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)

                    txtPaidAmount.Text = "0.00"
                    txtNotes.Text = "سداد من حساب الطالب"
                    cmbStudents_SelectionChanged(Nothing, Nothing)
                    LoadRecentPayments()
                Catch ex As Exception
                    trans.Rollback()
                    Throw ex
                End Try
            End Using
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء حفظ سند القبض: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub LoadRecentPayments()
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT TOP 20 T.TransactionID, S.StudentName, T.Credit, T.Notes, T.TransactionDate, U.Username " &
                                     "FROM FinancialTransactions T " &
                                     "INNER JOIN Students S ON T.StudentID = S.StudentID " &
                                     "INNER JOIN Users U ON T.UserID = U.UserID " &
                                     "WHERE T.TransactionType = 'Payment Receipt' " &
                                     "ORDER BY T.TransactionID DESC"
                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.Fill(dt)
                End Using
            End Using

            dgPayments.ItemsSource = dt.DefaultView
        Catch ex As Exception
            ' Handling error
        End Try
    End Sub
End Class
