Imports System
Imports System.Data.SqlClient

Public Class SettingsService
    Public Class CenterSettings
        Public Property CenterName As String = "منظومة مركز الدورات التعليمية"
        Public Property LogoData As Byte() = Nothing
    End Class

    Public Shared Function GetSettings() As CenterSettings
        Dim settings As New CenterSettings()
        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT CenterName, LogoData FROM SystemSettings WHERE SettingID = 1"
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            If Not reader.IsDBNull(0) Then
                                settings.CenterName = reader.GetString(0)
                            End If
                            If Not reader.IsDBNull(1) Then
                                settings.LogoData = CType(reader.GetValue(1), Byte())
                            End If
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Fallback to defaults
        End Try
        Return settings
    End Function

    Public Shared Function SaveSettings(centerName As String, logoData As Byte()) As Boolean
        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "UPDATE SystemSettings SET CenterName = @CenterName, LogoData = @LogoData WHERE SettingID = 1"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@CenterName", centerName)
                    cmd.Parameters.AddWithValue("@LogoData", If(logoData IsNot Nothing, CType(logoData, Object), DBNull.Value))
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
