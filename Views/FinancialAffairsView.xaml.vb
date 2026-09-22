Imports System.Windows
Imports System.Windows.Controls

Public Class FinancialAffairsView
    Private Sub btnTreasuryStatement_Click(sender As Object, e As RoutedEventArgs)
        Dim mainWin As MainWindow = TryCast(Window.GetWindow(Me), MainWindow)
        If mainWin IsNot Nothing Then
            mainWin.NavigateTo("BalanceReport")
        End If
    End Sub

    Private Sub btnStudentStatement_Click(sender As Object, e As RoutedEventArgs)
        Dim mainWin As MainWindow = TryCast(Window.GetWindow(Me), MainWindow)
        If mainWin IsNot Nothing Then
            mainWin.NavigateTo("BalanceReport")
        End If
    End Sub

    Private Sub btnReceiptVouchersList_Click(sender As Object, e As RoutedEventArgs)
        Dim mainWin As MainWindow = TryCast(Window.GetWindow(Me), MainWindow)
        If mainWin IsNot Nothing Then
            mainWin.NavigateTo("Payments")
        End If
    End Sub

    Private Sub btnPaymentVouchersList_Click(sender As Object, e As RoutedEventArgs)
        Dim mainWin As MainWindow = TryCast(Window.GetWindow(Me), MainWindow)
        If mainWin IsNot Nothing Then
            mainWin.NavigateTo("Payments")
        End If
    End Sub

    Private Sub btnNewReceiptVoucher_Click(sender As Object, e As RoutedEventArgs)
        Dim mainWin As MainWindow = TryCast(Window.GetWindow(Me), MainWindow)
        If mainWin IsNot Nothing Then
            mainWin.NavigateTo("Payments")
        End If
    End Sub

    Private Sub btnNewPaymentVoucher_Click(sender As Object, e As RoutedEventArgs)
        Dim mainWin As MainWindow = TryCast(Window.GetWindow(Me), MainWindow)
        If mainWin IsNot Nothing Then
            mainWin.NavigateTo("Payments")
        End If
    End Sub
End Class
