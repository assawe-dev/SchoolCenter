Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows

Public Class AccountStatementWindow
    Private studentID As Integer
    Private studentName As String

    Public Sub New(id As Integer, name As String)
        InitializeComponent()
        Me.studentID = id
        Me.studentName = name
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        txtStudentTitle.Text = "كشف حساب الطالب: " & studentName & " (رقم: " & studentID & ")"
        LoadStatementData()
    End Sub

    Private Sub LoadStatementData()
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT T.TransactionID, T.TransactionType, T.Debit, T.Credit, T.TransactionDate, T.Notes, U.Username " &
                                     "FROM FinancialTransactions T " &
                                     "INNER JOIN Users U ON T.UserID = U.UserID " &
                                     "WHERE T.StudentID = @StudentID " &
                                     "ORDER BY T.TransactionDate ASC, T.TransactionID ASC"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@StudentID", studentID)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            dt.Columns.Add("TranslatedType", GetType(String))
            dt.Columns.Add("RunningBalance", GetType(Decimal))

            Dim totalDebit As Decimal = 0
            Dim totalCredit As Decimal = 0
            Dim cumulativeBalance As Decimal = 0

            For Each row As DataRow In dt.Rows
                Dim debitVal As Decimal = Convert.ToDecimal(row("Debit"))
                Dim creditVal As Decimal = Convert.ToDecimal(row("Credit"))

                totalDebit += debitVal
                totalCredit += creditVal
                cumulativeBalance += (debitVal - creditVal)

                row("RunningBalance") = cumulativeBalance

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

            txtTotalDebit.Text = totalDebit.ToString("N2") & " د.ل"
            txtTotalCredit.Text = totalCredit.ToString("N2") & " د.ل"
            txtFinalBalance.Text = cumulativeBalance.ToString("N2") & " د.ل"

            dgStatement.ItemsSource = dt.DefaultView
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل كشف الحساب: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class
