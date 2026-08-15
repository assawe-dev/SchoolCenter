Imports System.Windows

Class App
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs)
        Try
            DbConnectionManager.InitializeDatabase()
        Catch ex As Exception
            MessageBox.Show("تحذير عند تهيئة قاعدة البيانات: " & ex.Message, "إشعارات المنظومة", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Sub
End Class
