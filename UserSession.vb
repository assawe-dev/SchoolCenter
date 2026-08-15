Public Class UserSession
    Public Shared Property CurrentUserID As Integer = 0
    Public Shared Property Username As String = String.Empty
    Public Shared Property Role As String = String.Empty

    ' الصلاحيات التفصيلية
    Public Shared Property CanManageStudents As Boolean = True
    Public Shared Property CanManageCourses As Boolean = True
    Public Shared Property CanAssignDues As Boolean = True
    Public Shared Property CanReceivePayments As Boolean = True
    Public Shared Property CanViewReports As Boolean = True
    Public Shared Property CanManageUsers As Boolean = True

    Public Shared Function IsLoggedIn() As Boolean
        Return CurrentUserID > 0
    End Function

    Public Shared Sub ClearSession()
        CurrentUserID = 0
        Username = String.Empty
        Role = String.Empty
        CanManageStudents = False
        CanManageCourses = False
        CanAssignDues = False
        CanReceivePayments = False
        CanViewReports = False
        CanManageUsers = False
    End Sub
End Class
