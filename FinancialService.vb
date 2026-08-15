Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class FinancialService
    ''' <summary>
    ''' جلب إجمالي عدد الطلاب في المنظومة
    ''' </summary>
    Public Shared Function GetTotalStudents() As Integer
        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT COUNT(*) FROM Students"
                Using cmd As New SqlCommand(query, conn)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
                        Return Convert.ToInt32(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Handling error gracefully
        End Try
        Return 0
    End Function

    ''' <summary>
    ''' جلب رصيد الخزينة الحالي (إجمالي المقبوضات المقبوضة)
    ''' </summary>
    Public Shared Function GetCurrentTreasuryBalance() As Decimal
        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT ISNULL(SUM(Credit), 0) FROM FinancialTransactions WHERE TransactionType = N'Payment Receipt'"
                Using cmd As New SqlCommand(query, conn)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
                        Return Convert.ToDecimal(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Handling error gracefully
        End Try
        Return 0.0D
    End Function

    ''' <summary>
    ''' جلب إجمالي الديون المستحقة على جميع الطلاب (إجمالي المدين - إجمالي الدائن)
    ''' </summary>
    Public Shared Function GetTotalOutstandingDebts() As Decimal
        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT ISNULL(SUM(Debit), 0) - ISNULL(SUM(Credit), 0) FROM FinancialTransactions"
                Using cmd As New SqlCommand(query, conn)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
                        Dim total As Decimal = Convert.ToDecimal(result)
                        If total > 0 Then
                            Return total
                        Else
                            Return 0.0D
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Handling error gracefully
        End Try
        Return 0.0D
    End Function

    ''' <summary>
    ''' جلب الرصيد الحالي لطالب معين (الديون أو الرصيد المتبقي عليه)
    ''' </summary>
    Public Shared Function GetStudentBalance(studentId As Integer) As Decimal
        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT ISNULL(SUM(Debit), 0) - ISNULL(SUM(Credit), 0) FROM FinancialTransactions WHERE StudentID = @StudentID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@StudentID", studentId)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
                        Return Convert.ToDecimal(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Handling error
        End Try
        Return 0.0D
    End Function
End Class
