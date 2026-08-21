Imports System
Imports System.IO
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media.Imaging
Imports Microsoft.Win32

Public Class SettingsView
    Private selectedLogoBytes As Byte() = Nothing

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        LoadCurrentSettings()
    End Sub

    Private Sub LoadCurrentSettings()
        Try
            Dim settings As SettingsService.CenterSettings = SettingsService.GetSettings()
            txtCenterName.Text = settings.CenterName
            selectedLogoBytes = settings.LogoData

            DisplayLogo(selectedLogoBytes)
        Catch ex As Exception
            ' Handling error
        End Try
    End Sub

    Private Sub DisplayLogo(bytes As Byte())
        If bytes IsNot Nothing AndAlso bytes.Length > 0 Then
            Try
                Dim ms As New MemoryStream(bytes)
                Dim bitmap As New BitmapImage()
                bitmap.BeginInit()
                bitmap.CacheOption = BitmapCacheOption.OnLoad
                bitmap.StreamSource = ms
                bitmap.EndInit()

                imgLogoPreview.Source = bitmap
                imgLogoPreview.Visibility = Visibility.Visible
                txtLogoPlaceholder.Visibility = Visibility.Collapsed
                Return
            Catch ex As Exception
            End Try
        End If

        imgLogoPreview.Source = Nothing
        imgLogoPreview.Visibility = Visibility.Collapsed
        txtLogoPlaceholder.Visibility = Visibility.Visible
    End Sub

    Private Sub btnUploadLogo_Click(sender As Object, e As RoutedEventArgs)
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "ملفات الصور (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
        If ofd.ShowDialog().GetValueOrDefault() Then
            Try
                selectedLogoBytes = File.ReadAllBytes(ofd.FileName)
                DisplayLogo(selectedLogoBytes)
            Catch ex As Exception
                MessageBox.Show("تعذر تحميل الصورة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End If
    End Sub

    Private Sub btnClearLogo_Click(sender As Object, e As RoutedEventArgs)
        selectedLogoBytes = Nothing
        DisplayLogo(Nothing)
    End Sub

    Private Sub btnSaveSettings_Click(sender As Object, e As RoutedEventArgs)
        Dim name As String = txtCenterName.Text.Trim()
        If String.IsNullOrEmpty(name) Then
            MessageBox.Show("يرجى أدخل اسم المركز.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        If SettingsService.SaveSettings(name, selectedLogoBytes) Then
            MessageBox.Show("تم حفظ إعدادات المركز بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)

            ' تحديث المسمى في MainWindow إذا كانت متوفرة
            Dim mainWin As MainWindow = TryCast(Window.GetWindow(CType(Me, DependencyObject)), MainWindow)
            If mainWin IsNot Nothing Then
                mainWin.RefreshCenterBranding()
            End If
        Else
            MessageBox.Show("حدث خطأ أثناء حفظ الإعدادات.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
    End Sub
End Class
