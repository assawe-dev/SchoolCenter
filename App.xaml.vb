Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media

Class App
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs)
        Try
            DbConnectionManager.InitializeDatabase()
        Catch ex As Exception
            MessageBox.Show("تحذير عند تهيئة قاعدة البيانات: " & ex.Message, "إشعارات المنظومة", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try

        RegisterEnterNavigationHandlers()
    End Sub

    Private Sub RegisterEnterNavigationHandlers()
        EventManager.RegisterClassHandler(GetType(TextBox), UIElement.KeyDownEvent, New KeyEventHandler(AddressOf OnTextBoxKeyDown))
        EventManager.RegisterClassHandler(GetType(PasswordBox), UIElement.KeyDownEvent, New KeyEventHandler(AddressOf OnPasswordBoxKeyDown))
    End Sub

    Private Sub OnTextBoxKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            Dim tb = TryCast(sender, TextBox)
            If tb IsNot Nothing Then
                If tb.AcceptsReturn Then Return
                If IsInsideDataGrid(tb) Then Return

                Dim uie = TryCast(sender, UIElement)
                If uie IsNot Nothing Then
                    uie.MoveFocus(New TraversalRequest(FocusNavigationDirection.Next))
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub OnPasswordBoxKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            Dim pb = TryCast(sender, PasswordBox)
            If pb IsNot Nothing Then
                If IsInsideDataGrid(pb) Then Return

                Dim uie = TryCast(sender, UIElement)
                If uie IsNot Nothing Then
                    uie.MoveFocus(New TraversalRequest(FocusNavigationDirection.Next))
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Function IsInsideDataGrid(element As DependencyObject) As Boolean
        Dim parent = VisualTreeHelper.GetParent(element)
        While parent IsNot Nothing
            If TypeOf parent Is DataGrid Then Return True
            parent = VisualTreeHelper.GetParent(parent)
        End While
        Return False
    End Function
End Class
