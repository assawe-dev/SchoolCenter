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
                Dim query As String = "SELECT CourseID, CourseName, Cost, TeacherName FROM Courses WHERE 1=1 "
                If Not String.IsNullOrWhiteSpace(filter) Then
                    query &= "AND (CourseName LIKE @Filter OR TeacherName LIKE @Filter) "
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
            txtTeacherName.Text = If(row("TeacherName") Is DBNull.Value, "", row("TeacherName").ToString())
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
        Dim teacher As String = txtTeacherName.Text.Trim()
        Dim cost As Decimal = 0

        If String.IsNullOrEmpty(name) OrElse String.IsNullOrEmpty(teacher) OrElse Not Decimal.TryParse(txtCourseCost.Text.Trim(), cost) Then
            MessageBox.Show("يرجى إدخال اسم الدورة، اسم المحاضر، والسعر بشكل صحيح.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        Try
            Using conn As New SqlConnection(DbConnectionManager.GetConnectionString())
                conn.Open()
                If selectedCourseID = 0 Then
                    Dim query As String = "INSERT INTO Courses (CourseName, Cost, TeacherName) VALUES (@CourseName, @Cost, @TeacherName)"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@CourseName", name)
                        cmd.Parameters.AddWithValue("@Cost", cost)
                        cmd.Parameters.AddWithValue("@TeacherName", teacher)
                        cmd.ExecuteNonQuery()
                    End Using
                    DbConnectionManager.LogAudit("إضافة دورة", "تم إضافة دورة تعليمية جديدة: " & name & " - المحاضر: " & teacher)
                    MessageBox.Show("تم إضافة الدورة التعليمية بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
                Else
                    Dim query As String = "UPDATE Courses SET CourseName = @CourseName, Cost = @Cost, TeacherName = @TeacherName WHERE CourseID = @CourseID"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@CourseName", name)
                        cmd.Parameters.AddWithValue("@Cost", cost)
                        cmd.Parameters.AddWithValue("@TeacherName", teacher)
                        cmd.Parameters.AddWithValue("@CourseID", selectedCourseID)
                        cmd.ExecuteNonQuery()
                    End Using
                    DbConnectionManager.LogAudit("تعديل دورة", "تم تحديث الدورة التعليمية رقم (" & selectedCourseID & "): " & name)
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

                DbConnectionManager.LogAudit("حذف دورة", "تم حذف الدورة التعليمية رقم (" & selectedCourseID & ")")
                MessageBox.Show("تم حذف الدورة بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information)
                LoadCoursesData()
                ResetForm()
            Catch ex As Exception
                MessageBox.Show("حدث خطأ أثناء حذف الدورة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim dv As DataView = CType(dgCourses.ItemsSource, DataView)
            If dv Is Nothing OrElse dv.Count = 0 Then Return
            Dim dt As DataTable = dv.ToTable()

            Dim cols As New Generic.List(Of ReportColumn)()
            cols.Add(New ReportColumn("رقم الدورة", "CourseID", 0.8))
            cols.Add(New ReportColumn("اسم الدورة التعليمية", "CourseName", 2.0))
            cols.Add(New ReportColumn("اسم المحاضر / الأستاذ", "TeacherName", 1.5))
            cols.Add(New ReportColumn("سعر الدورة", "Cost", 1.2))

            Dim doc As Documents.FlowDocument = PrintingService.CreateReportDocument("دليل الدورات التعليمية", Nothing, dt, cols, "قائمة الكورسات والأسعار والمحاضرين")
            PrintingService.PrintDocument(doc, "دليل الدورات")
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء الطباعة: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnExport_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim dv As DataView = CType(dgCourses.ItemsSource, DataView)
            If dv Is Nothing OrElse dv.Count = 0 Then Return
            Dim dt As DataTable = dv.ToTable()
            PrintingService.ExportDataTableToCSV(dt, "قائمة_الدورات")
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء التصدير: " & ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub btnCancelEdit_Click(sender As Object, e As RoutedEventArgs)
        ResetForm()
    End Sub

    Private Sub ResetForm()
        selectedCourseID = 0
        txtCourseName.Text = ""
        txtTeacherName.Text = ""
        txtCourseCost.Text = "0.00"
        txtFormTitle.Text = "إضافة دورة جديدة"
        btnDeleteCourse.Visibility = Visibility.Collapsed
        dgCourses.UnselectAll()
    End Sub
End Class
