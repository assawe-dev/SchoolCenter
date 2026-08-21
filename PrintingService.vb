Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Media

Public Class StatItem
    Public Property Label As String
    Public Property Value As String
    Public Property TextColor As Brush

    Public Sub New(label As String, value As String, Optional textColor As Brush = Nothing)
        Me.Label = label
        Me.Value = value
        Me.TextColor = If(textColor, Brushes.Black)
    End Sub
End Class

Public Class ReportColumn
    Public Property Header As String
    Public Property PropertyName As String
    Public Property Width As GridLength

    Public Sub New(header As String, propertyName As String, Optional width As Double = 1.0)
        Me.Header = header
        Me.PropertyName = propertyName
        Me.Width = New GridLength(width, GridUnitType.Star)
    End Sub
End Class

Public Class PrintingService
    ''' <summary>
    ''' Builds a standard FlowDocument for report printing with header, metadata, statistics summary, and a data table.
    ''' </summary>
    Public Shared Function CreateReportDocument(title As String,
                                                stats As List(Of StatItem),
                                                dataTable As DataTable,
                                                columns As List(Of ReportColumn),
                                                Optional subtitle As String = "") As FlowDocument
        Dim doc As New FlowDocument()
        doc.FlowDirection = FlowDirection.RightToLeft
        doc.PagePadding = New Thickness(40)
        doc.ColumnWidth = 999999 ' Prevent multi-column page flow
        doc.FontFamily = New FontFamily("Segoe UI, Tahoma, Arial")

        Dim settings As SettingsService.CenterSettings = SettingsService.GetSettings()
        Dim centerName As String = If(Not String.IsNullOrEmpty(settings.CenterName), settings.CenterName, "منظومة المركز التعليمي")

        ' --- HEADER ---
        Dim headerTable As New Table()
        headerTable.Margin = New Thickness(0, 0, 0, 15)
        headerTable.Columns.Add(New TableColumn() With {.Width = New GridLength(2, GridUnitType.Star)})
        headerTable.Columns.Add(New TableColumn() With {.Width = New GridLength(1, GridUnitType.Star)})

        Dim headerGroup As New TableRowGroup()
        Dim headerRow As New TableRow()

        ' Left/Right Header Info (Center Name and Date)
        Dim centerPara As New Paragraph()
        centerPara.Inlines.Add(New Run(centerName) With {.FontSize = 18, .FontWeight = FontWeights.Bold, .Foreground = CType(Application.Current.Resources("PrimaryBrush"), Brush)})
        If Not String.IsNullOrEmpty(subtitle) Then
            centerPara.Inlines.Add(New LineBreak())
            centerPara.Inlines.Add(New Run(subtitle) With {.FontSize = 11, .Foreground = Brushes.Gray})
        End If

        Dim cellCenter As New TableCell(centerPara)
        cellCenter.TextAlignment = TextAlignment.Right

        Dim metaPara As New Paragraph()
        metaPara.Inlines.Add(New Run("تاريخ التقرير: " & DateTime.Now.ToString("yyyy-MM-dd HH:mm")) With {.FontSize = 11, .Foreground = Brushes.DimGray})
        metaPara.Inlines.Add(New LineBreak())
        metaPara.Inlines.Add(New Run("المستخدم: " & UserSession.Username) With {.FontSize = 11, .Foreground = Brushes.DimGray})

        Dim cellMeta As New TableCell(metaPara)
        cellMeta.TextAlignment = TextAlignment.Left

        headerRow.Cells.Add(cellCenter)
        headerRow.Cells.Add(cellMeta)
        headerGroup.Rows.Add(headerRow)
        headerTable.RowGroups.Add(headerGroup)
        doc.Blocks.Add(headerTable)

        ' Divider
        doc.Blocks.Add(New BlockUIContainer(New Border() With {
            .BorderBrush = CType(Application.Current.Resources("BorderBrushLight"), Brush),
            .BorderThickness = New Thickness(0, 0, 0, 1),
            .Margin = New Thickness(0, 0, 0, 15)
        }))

        ' --- TITLE ---
        Dim titlePara As New Paragraph(New Run(title))
        titlePara.FontSize = 16
        titlePara.FontWeight = FontWeights.Bold
        titlePara.TextAlignment = TextAlignment.Center
        titlePara.Margin = New Thickness(0, 0, 0, 15)
        doc.Blocks.Add(titlePara)

        ' --- SUMMARY STATISTICS CARDS ---
        If stats IsNot Nothing AndAlso stats.Count > 0 Then
            Dim statsGrid As New Grid()
            statsGrid.Margin = New Thickness(0, 0, 0, 20)

            For i As Integer = 0 To stats.Count - 1
                statsGrid.ColumnDefinitions.Add(New ColumnDefinition() With {.Width = New GridLength(1, GridUnitType.Star)})
            Next

            For i As Integer = 0 To stats.Count - 1
                Dim stat As StatItem = stats(i)
                Dim border As New Border()
                border.Background = CType(Application.Current.Resources("CardBackgroundBrush"), Brush)
                border.BorderBrush = CType(Application.Current.Resources("BorderBrushLight"), Brush)
                border.BorderThickness = New Thickness(1)
                border.CornerRadius = New CornerRadius(6)
                border.Padding = New Thickness(10)
                border.Margin = New Thickness(4, 0, 4, 0)

                Dim sp As New StackPanel()
                Dim lblBlock As New TextBlock() With {
                    .Text = stat.Label,
                    .FontSize = 11,
                    .Foreground = Brushes.Gray,
                    .HorizontalAlignment = HorizontalAlignment.Center
                }
                Dim valBlock As New TextBlock() With {
                    .Text = stat.Value,
                    .FontSize = 15,
                    .FontWeight = FontWeights.Bold,
                    .Foreground = stat.TextColor,
                    .HorizontalAlignment = HorizontalAlignment.Center,
                    .Margin = New Thickness(0, 4, 0, 0)
                }

                sp.Children.Add(lblBlock)
                sp.Children.Add(valBlock)
                border.Child = sp

                Grid.SetColumn(border, i)
                statsGrid.Children.Add(border)
            Next

            doc.Blocks.Add(New BlockUIContainer(statsGrid))
        End If

        ' --- DATA TABLE ---
        If dataTable IsNot Nothing AndAlso columns IsNot Nothing AndAlso columns.Count > 0 Then
            Dim gridTable As New Table()
            gridTable.CellSpacing = 0
            gridTable.BorderThickness = New Thickness(1)
            gridTable.BorderBrush = CType(Application.Current.Resources("BorderBrushLight"), Brush)

            For Each col As ReportColumn In columns
                gridTable.Columns.Add(New TableColumn() With {.Width = col.Width})
            Next

            ' Table Header
            Dim tableRowGroup As New TableRowGroup()
            Dim tableHeaderRow As New TableRow()
            tableHeaderRow.Background = CType(Application.Current.Resources("DarkNavyBrush"), Brush)

            For Each col As ReportColumn In columns
                Dim p As New Paragraph(New Run(col.Header))
                p.FontSize = 11
                p.FontWeight = FontWeights.Bold
                p.Foreground = Brushes.White
                p.TextAlignment = TextAlignment.Center
                p.Margin = New Thickness(6)

                Dim cell As New TableCell(p)
                cell.BorderBrush = CType(Application.Current.Resources("BorderBrushLight"), Brush)
                cell.BorderThickness = New Thickness(0, 0, 1, 1)
                tableHeaderRow.Cells.Add(cell)
            Next
            tableRowGroup.Rows.Add(tableHeaderRow)

            ' Table Data Rows
            Dim rowIndex As Integer = 0
            For Each dr As DataRow In dataTable.Rows
                Dim row As New TableRow()
                If rowIndex Mod 2 = 1 Then
                    row.Background = New SolidColorBrush(Color.FromRgb(248, 250, 252))
                Else
                    row.Background = Brushes.White
                End If

                For Each col As ReportColumn In columns
                    Dim valStr As String = ""
                    If dataTable.Columns.Contains(col.PropertyName) AndAlso Not dr.IsNull(col.PropertyName) Then
                        Dim rawVal As Object = dr(col.PropertyName)
                        If TypeOf rawVal Is Decimal OrElse TypeOf rawVal Is Double OrElse TypeOf rawVal Is Single Then
                            valStr = String.Format("{0:N2}", rawVal)
                        ElseIf TypeOf rawVal Is DateTime Then
                            valStr = CType(rawVal, DateTime).ToString("yyyy-MM-dd HH:mm")
                        Else
                            valStr = rawVal.ToString()
                        End If
                    End If

                    Dim p As New Paragraph(New Run(valStr))
                    p.FontSize = 10.5
                    p.TextAlignment = TextAlignment.Center
                    p.Margin = New Thickness(6)

                    Dim cell As New TableCell(p)
                    cell.BorderBrush = CType(Application.Current.Resources("BorderBrushLight"), Brush)
                    cell.BorderThickness = New Thickness(0, 0, 1, 1)
                    row.Cells.Add(cell)
                Next

                tableRowGroup.Rows.Add(row)
                rowIndex += 1
            Next

            gridTable.RowGroups.Add(tableRowGroup)
            doc.Blocks.Add(gridTable)
        End If

        Return doc
    End Function

    ''' <summary>
    ''' Sends a FlowDocument to the printer using PrintDialog.
    ''' </summary>
    Public Shared Sub PrintDocument(doc As FlowDocument, description As String)
        Dim pd As New PrintDialog()
        If pd.ShowDialog().GetValueOrDefault() Then
            Dim paginator As IDocumentPaginatorSource = doc
            pd.PrintDocument(paginator.DocumentPaginator, description)
        End If
    End Sub
End Class
