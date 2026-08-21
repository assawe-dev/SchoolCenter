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

    Private Sub btnPrintStatement_Click(sender As Object, e As RoutedEventArgs)
        Dim dv As DataView = TryCast(dgStatement.ItemsSource, DataView)
        If dv Is Nothing OrElse dv.Table Is Nothing OrElse dv.Table.Rows.Count = 0 Then
            MessageBox.Show("لا توجد بيانات حركة مالية للطباعة.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Information)
            Return
        End If

        Try
            Dim dt As DataTable = dv.Table

            ' Calculate totals
            Dim totalDebit As Decimal = 0
            Dim totalCredit As Decimal = 0
            For Each row As DataRow In dt.Rows
                totalDebit += Convert.ToDecimal(row("Debit"))
                totalCredit += Convert.ToDecimal(row("Credit"))
            Next
            Dim finalBalance As Decimal = totalDebit - totalCredit

            ' Summary stats
            Dim stats As New System.Collections.Generic.List(Of StatItem)()
            stats.Add(New StatItem("إجمالي المطلوب (مدين)", totalDebit.ToString("N2") & " د.ل", CType(Application.Current.Resources("DangerBrush"), System.Windows.Media.Brush)))
            stats.Add(New StatItem("إجمالي المدفوع (دائن)", totalCredit.ToString("N2") & " د.ل", CType(Application.Current.Resources("SuccessBrush"), System.Windows.Media.Brush)))
            stats.Add(New StatItem("الرصيد المتبقي النهائي", finalBalance.ToString("N2") & " د.ل", CType(Application.Current.Resources("PrimaryBrush"), System.Windows.Media.Brush)))

            ' Columns definition
            Dim cols As New System.Collections.Generic.List(Of ReportColumn)()
            cols.Add(New ReportColumn("تاريخ الحركة", "TransactionDate", 1.3))
            cols.Add(New ReportColumn("نوع الحركة", "TranslatedType", 1.2))
            cols.Add(New ReportColumn("البيان / ملاحظات", "Notes", 2.2))
            cols.Add(New ReportColumn("المطلوب (مدين)", "Debit", 1.1))
            cols.Add(New ReportColumn("المدفوع (دائن)", "Credit", 1.1))
            cols.Add(New ReportColumn("الرصيد التراكمي", "RunningBalance", 1.2))
            cols.Add(New ReportColumn("الموظف", "Username", 1.0))

            Dim reportTitle As String = "كشف حساب تفصيلي للطالب: " & studentName
            Dim subtitle As String = "رقم الطالب: " & studentID

            Dim doc As System.Windows.Documents.FlowDocument = PrintingService.CreateReportDocument(reportTitle, stats, dt, cols, subtitle)
            PrintingService.PrintDocument(doc, "كشف حساب الطالب " & studentName)
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء إعداد كشف الحساب للطباعة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class
