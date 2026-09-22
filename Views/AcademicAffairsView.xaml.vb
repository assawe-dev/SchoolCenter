Imports System.Windows
Imports System.Windows.Controls

Public Class AcademicAffairsView
    Private Sub btnComprehensiveStudentList_Click(sender As Object, e As RoutedEventArgs)
        Dim mainWin As MainWindow = TryCast(Window.GetWindow(Me), MainWindow)
        If mainWin IsNot Nothing Then
            mainWin.NavigateTo("Students")
        End If
    End Sub

    Private Sub btnCourses_Click(sender As Object, e As RoutedEventArgs)
        Dim mainWin As MainWindow = TryCast(Window.GetWindow(Me), MainWindow)
        If mainWin IsNot Nothing Then
            mainWin.NavigateTo("Courses")
        End If
    End Sub

    Private Sub btnStudentsWithCourses_Click(sender As Object, e As RoutedEventArgs)
        Dim mainWin As MainWindow = TryCast(Window.GetWindow(Me), MainWindow)
        If mainWin IsNot Nothing Then
            mainWin.NavigateTo("StudentDues")
        End If
    End Sub
End Class
