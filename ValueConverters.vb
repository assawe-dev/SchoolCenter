Imports System
Imports System.Globalization
Imports System.IO
Imports System.Windows.Data
Imports System.Windows.Media.Imaging

Public Class ByteArrayToImageConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If value IsNot Nothing AndAlso TypeOf value Is Byte() Then
            Dim bytes = CType(value, Byte())
            If bytes.Length > 0 Then
                Try
                    Dim ms As New MemoryStream(bytes)
                    Dim image As New BitmapImage()
                    image.BeginInit()
                    image.CacheOption = BitmapCacheOption.OnLoad
                    image.StreamSource = ms
                    image.EndInit()
                    Return image
                Catch ex As Exception
                    Return Nothing
                End Try
            End If
        End If
        Return Nothing
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class

Public Class CurrencyFormatter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If value IsNot Nothing AndAlso IsNumeric(value) Then
            Dim amount As Decimal = System.Convert.ToDecimal(value)
            Return amount.ToString("N2") & " د.ل"
        End If
        Return "0.00 د.ل"
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
