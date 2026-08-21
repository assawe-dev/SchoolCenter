Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows
Imports System.Windows.Controls

Public Class DashboardView
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        LoadDashboardMetrics()
        LoadRecentTransactions()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As RoutedEventArgs)
        LoadDashboardMetrics()
        LoadRecentTransactions()
    End Sub

    Private Sub LoadDashboardMetrics()
        Try
            txtTotalStudents.Text = FinancialService.GetTotalStudents().ToString()
            txtTreasuryBalance.Text = FinancialService.GetCurrentTreasuryBalance().ToString("N2") & " د.ل"
            txtTotalDebts.Text = FinancialService.GetTotalOutstandingDebts().ToString("N2") & " د.ل"

            ' جلب عدد الدورات
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim cmd As New SqlCommand("SELECT COUNT(*) FROM Courses", conn)
                Dim count As Object = cmd.ExecuteScalar()
                txtTotalCourses.Text = If(count IsNot DBNull.Value AndAlso count IsNot Nothing, count.ToString(), "0")
            End Using
        Catch ex As Exception
            ' Handling error
        End Try
    End Sub

    Private Sub LoadRecentTransactions()
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT TOP 20 T.TransactionID, S.StudentName, T.TransactionType, T.Debit, T.Credit, T.TransactionDate, U.Username " &
                                     "FROM FinancialTransactions T " &
                                     "INNER JOIN Students S ON T.StudentID = S.StudentID " &
                                     "INNER JOIN Users U ON T.UserID = U.UserID " &
                                     "ORDER BY T.TransactionID DESC"
                Using adapter As New SqlDataAdapter(query, conn)
                    adapter.Fill(dt)
                End Using
            End Using

            ' إضافة عمود النوع المترجم
            dt.Columns.Add("TranslatedType", GetType(String))
            For Each row As DataRow In dt.Rows
                Dim typeStr As String = row("TransactionType").ToString()
                Select Case typeStr
                    Case "Fee Charge"
                        row("TranslatedType") = "استحقاق دورة"
                    Case "Payment Receipt"
                        row("TranslatedType") = "سند قبض"
                    Case "Opening Balance"
                        row("TranslatedType") = "رصيد افتتاحي"
                    Case Else
                        row("TranslatedType") = typeStr
                End Select
            Next

            dgRecentTransactions.ItemsSource = dt.DefaultView
        Catch ex As Exception
            ' Handling error
        End Try
    End Sub
End Class
