Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports Microsoft.Win32

Public Class BalanceReportView
    Private dtReport As New DataTable()

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        LoadReportData()
    End Sub

    Private Sub LoadReportData()
        Try
            Dim filter As String = txtSearch.Text.Trim()
            Dim debtorsOnly As Boolean = (chkDebtorsOnly.IsChecked = True)

            dtReport = New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT S.StudentID, S.StudentName, S.GuardianName, S.ParentPhone, " &
                                     "ISNULL(SUM(T.Debit), 0) AS TotalDebit, " &
                                     "ISNULL(SUM(T.Credit), 0) AS TotalCredit, " &
                                     "ISNULL(SUM(T.Debit), 0) - ISNULL(SUM(T.Credit), 0) AS RemainingBalance " &
                                     "FROM Students S " &
                                     "LEFT JOIN FinancialTransactions T ON S.StudentID = T.StudentID " &
                                     "WHERE 1=1 "

                If Not String.IsNullOrWhiteSpace(filter) Then
                    query &= "AND (S.StudentName LIKE @Filter OR S.GuardianName LIKE @Filter OR S.ParentPhone LIKE @Filter) "
                End If

                query &= "GROUP BY S.StudentID, S.StudentName, S.GuardianName, S.ParentPhone "

                If debtorsOnly Then
                    query &= "HAVING (ISNULL(SUM(T.Debit), 0) - ISNULL(SUM(T.Credit), 0)) > 0 "
                End If

                query &= "ORDER BY RemainingBalance DESC, S.StudentName ASC"

                Using cmd As New SqlCommand(query, conn)
                    If Not String.IsNullOrWhiteSpace(filter) Then
                        cmd.Parameters.AddWithValue("@Filter", "%" & filter & "%")
                    End If
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dtReport)
                    End Using
                End Using
            End Using

            ' حساب الإجماليات
            Dim sumDebit As Decimal = 0
            Dim sumCredit As Decimal = 0
            Dim sumNet As Decimal = 0

            For Each row As DataRow In dtReport.Rows
                sumDebit += Convert.ToDecimal(row("TotalDebit"))
                sumCredit += Convert.ToDecimal(row("TotalCredit"))
                sumNet += Convert.ToDecimal(row("RemainingBalance"))
            Next

            txtTotalDebitSum.Text = sumDebit.ToString("N2") & " د.ل"
            txtTotalCreditSum.Text = sumCredit.ToString("N2") & " د.ل"
            txtNetBalanceSum.Text = sumNet.ToString("N2") & " د.ل"

            dgBalanceReport.ItemsSource = dtReport.DefaultView
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل تقرير الأرصدة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As TextChangedEventArgs)
        LoadReportData()
    End Sub

    Private Sub chkDebtorsOnly_CheckedChanged(sender As Object, e As RoutedEventArgs)
        LoadReportData()
    End Sub

    Private Sub btnPrintReport_Click(sender As Object, e As RoutedEventArgs)
        If dtReport Is Nothing OrElse dtReport.Rows.Count = 0 Then
            MessageBox.Show("لا توجد بيانات متاحة للطباعة.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Information)
            Return
        End If

        Try
            Dim sumDebit As Decimal = 0
            Dim sumCredit As Decimal = 0
            Dim sumNet As Decimal = 0

            For Each row As DataRow In dtReport.Rows
                sumDebit += Convert.ToDecimal(row("TotalDebit"))
                sumCredit += Convert.ToDecimal(row("TotalCredit"))
                sumNet += Convert.ToDecimal(row("RemainingBalance"))
            Next

            Dim stats As New List(Of StatItem)()
            stats.Add(New StatItem("إجمالي المستحقات المترتبة", sumDebit.ToString("N2") & " د.ل", CType(Application.Current.Resources("TextPrimaryBrush"), Brush)))
            stats.Add(New StatItem("إجمالي المقبوضات المباشرة", sumCredit.ToString("N2") & " د.ل", CType(Application.Current.Resources("SuccessBrush"), Brush)))
            stats.Add(New StatItem("إجمالي الديون القائمة المستحقة", sumNet.ToString("N2") & " د.ل", CType(Application.Current.Resources("DangerBrush"), Brush)))

            Dim cols As New List(Of ReportColumn)()
            cols.Add(New ReportColumn("رقم الطالب", "StudentID", 0.8))
            cols.Add(New ReportColumn("اسم الطالب", "StudentName", 2.0))
            cols.Add(New ReportColumn("اسم ولي الأمر", "GuardianName", 1.5))
            cols.Add(New ReportColumn("رقم الهاتف", "ParentPhone", 1.2))
            cols.Add(New ReportColumn("إجمالي المستحق", "TotalDebit", 1.2))
            cols.Add(New ReportColumn("إجمالي المسدد", "TotalCredit", 1.2))
            cols.Add(New ReportColumn("الرصيد المتبقي", "RemainingBalance", 1.2))

            Dim doc = PrintingService.CreateReportDocument("تقرير أرصدة وحسابات الطلاب الشامل", stats, dtReport, cols, "تقرير مالي للأرصدة المستحقة والمقبوضة")
            PrintingService.PrintDocument(doc, "تقرير أرصدة الطلاب")
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء إعداد التقرير للطباعة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnExportCSV_Click(sender As Object, e As RoutedEventArgs)
        If dtReport Is Nothing OrElse dtReport.Rows.Count = 0 Then
            MessageBox.Show("لا توجد بيانات متاحة للتصدير.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Information)
            Return
        End If

        Dim sfd As New SaveFileDialog()
        sfd.Filter = "ملف Excel CSV (*.csv)|*.csv"
        sfd.FileName = "تقرير_أرصدة_الطلاب_" & DateTime.Now.ToString("yyyyMMdd_HHmm") & ".csv"

        If sfd.ShowDialog() = True Then
            Try
                ' الكتابة باستخدام UTF-8 BOM لمنع تشوه النصوص العربية في Excel
                Using writer As New StreamWriter(sfd.FileName, False, New UTF8Encoding(True))
                    ' رؤوس الأعمدة
                    writer.WriteLine("رقم الطالب,اسم الطالب,اسم ولي الأمر,رقم الهاتف,إجمالي المستحق (مدين),إجمالي المسدد (دائن),الرصيد المتبقي")

                    For Each row As DataRow In dtReport.Rows
                        Dim line As String = String.Format("""{0}"",""{1}"",""{2}"",""{3}"",""{4:N2}"",""{5:N2}"",""{6:N2}""",
                            row("StudentID"),
                            row("StudentName").ToString().Replace("""", """"""),
                            row("GuardianName").ToString().Replace("""", """"""),
                            row("ParentPhone").ToString().Replace("""", """"""),
                            Convert.ToDecimal(row("TotalDebit")),
                            Convert.ToDecimal(row("TotalCredit")),
                            Convert.ToDecimal(row("RemainingBalance")))
                        writer.WriteLine(line)
                    Next
                End Using

                MessageBox.Show("تم تصدير التقرير بنجاح إلى الملف:\n" & sfd.FileName, "نجاح التصدير", MessageBoxButton.OK, MessageBoxImage.Information)
            Catch ex As Exception
                MessageBox.Show("حدث خطأ أثناء تصدير الملف: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End If
    End Sub
End Class
