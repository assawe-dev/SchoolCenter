Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows
Imports System.Windows.Controls

Public Class CoursesView
    Private selectedCourseID As Integer = 0

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        LoadCoursesData()
        ResetForm()
    End Sub

    Private Sub LoadCoursesData(Optional filter As String = "")
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT CourseID, CourseName, Cost FROM Courses WHERE 1=1 "
                If Not String.IsNullOrWhiteSpace(filter) Then
                    query &= "AND CourseName LIKE @Filter "
                End If
                query &= "ORDER BY CourseID DESC"

                Using cmd As New SqlCommand(query, conn)
                    If Not String.IsNullOrWhiteSpace(filter) Then
                        cmd.Parameters.AddWithValue("@Filter", "%" & filter.Trim() & "%")
                    End If
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            dgCourses.ItemsSource = dt.DefaultView
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل بيانات الدورات: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As TextChangedEventArgs)
        LoadCoursesData(txtSearch.Text)
    End Sub

    Private Sub dgCourses_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If dgCourses.SelectedItem IsNot Nothing Then
            Dim row As DataRowView = CType(dgCourses.SelectedItem, DataRowView)
            selectedCourseID = Convert.ToInt32(row("CourseID"))
            txtCourseName.Text = row("CourseName").ToString()
            txtCourseCost.Text = Convert.ToDecimal(row("Cost")).ToString("F2")

            txtFormTitle.Text = "تعديل الدورة (" & selectedCourseID & ")"
            btnDeleteCourse.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub btnAddNewCourse_Click(sender As Object, e As RoutedEventArgs)
        ResetForm()
    End Sub

    Private Sub btnSaveCourse_Click(sender As Object, e As RoutedEventArgs)
        Dim name As String = txtCourseName.Text.Trim()
        Dim cost As Decimal = 0

        If String.IsNullOrEmpty(name) OrElse Not Decimal.TryParse(txtCourseCost.Text.Trim(), cost) Then
            MessageBox.Show("يرجى إدخال اسم الدورة والتكلفة بشكل صحيح.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                If selectedCourseID = 0 Then
                    Dim query As String = "INSERT INTO Courses (CourseName, Cost) VALUES (@CourseName, @Cost)"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@CourseName", name)
                        cmd.Parameters.AddWithValue("@Cost", cost)
                        cmd.ExecuteNonQuery()
                    End Using
                    MessageBox.Show("تم إضافة الدورة التعليمية بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
                Else
                    Dim query As String = "UPDATE Courses SET CourseName = @CourseName, Cost = @Cost WHERE CourseID = @CourseID"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@CourseName", name)
                        cmd.Parameters.AddWithValue("@Cost", cost)
                        cmd.Parameters.AddWithValue("@CourseID", selectedCourseID)
                        cmd.ExecuteNonQuery()
                    End Using
                    MessageBox.Show("تم تحديث الدورة التعليمية بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            End Using

            LoadCoursesData()
            ResetForm()
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء حفظ الدورة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnDeleteCourse_Click(sender As Object, e As RoutedEventArgs)
        If selectedCourseID = 0 Then Return

        If MessageBox.Show("هل أنت متاكد من حذف هذه الدورة؟", "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Warning) = MessageBoxResult.Yes Then
            Try
                Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                    conn.Open()
                    Dim query As String = "DELETE FROM Courses WHERE CourseID = @CourseID"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@CourseID", selectedCourseID)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show("تم حذف الدورة بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
                LoadCoursesData()
                ResetForm()
            Catch ex As Exception
                MessageBox.Show("حدث خطأ أثناء حذف الدورة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End If
    End Sub

    Private Sub btnCancelEdit_Click(sender As Object, e As RoutedEventArgs)
        ResetForm()
    End Sub

    Private Sub ResetForm()
        selectedCourseID = 0
        txtCourseName.Text = ""
        txtCourseCost.Text = "0.00"
        txtFormTitle.Text = "إضافة دورة جديدة"
        btnDeleteCourse.Visibility = Visibility.Collapsed
        dgCourses.UnselectAll()
    End Sub
End Class
