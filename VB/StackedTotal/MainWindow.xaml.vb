Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports DevExpress.Xpf.Charts

Namespace StackedTotal
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Property Totals() As Dictionary(Of String, PointInfo)
        Private SeriesColors As New List(Of SolidColorBrush) From {Brushes.Yellow, Brushes.Orange, Brushes.LightYellow}

        Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Totals = New Dictionary(Of String, PointInfo)()
            AddHandler Me.chartControl.BoundDataChanged, AddressOf chartControl_BoundDataChanged
            Me.chartControl.Diagram.SeriesDataMember = "Month"
            Me.chartControl.Diagram.SeriesTemplate = New BarStackedSeries2D()
            Me.chartControl.Diagram.SeriesTemplate.ArgumentDataMember = "Section"
            Me.chartControl.Diagram.SeriesTemplate.ValueDataMember = "Value"
            Me.chartControl.DataSource = CreateChartData()
        End Sub


        Private Sub chartControl_BoundDataChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Totals.Clear()
            Dim i As Integer = 0

            For Each series As XYSeries2D In chartControl.Diagram.Series
                If series.Tag.ToString() <> "CustomTotal" Then
                    series.Brush = SeriesColors(i)
                    i += 1
                    For Each sp As SeriesPoint In series.Points
                        If Not Totals.ContainsKey(sp.Argument) Then
                            Totals.Add(sp.Argument, New PointInfo() With { _
                                .ActualValue = sp.Value, _
                                .DisplayValue = If(sp.Value > 0, sp.Value, 0) _
                            })
                        Else
                            Dim info As PointInfo = Totals(sp.Argument)
                            info.ActualValue += sp.Value
                            If sp.Value > 0 Then
                            info.DisplayValue += sp.Value
                            End If
                            Totals(sp.Argument) = info
                        End If
                    Next sp
                End If
            Next series
        End Sub

        Private Function CreateChartData() As DataTable

            Dim table As New DataTable("Table1")

            table.Columns.Add("Month", GetType(String))
            table.Columns.Add("Section", GetType(String))
            table.Columns.Add("Value", GetType(Int32))

            table.Rows.Add(New Object() { "Jan", "Section1", 10 })
            table.Rows.Add(New Object() { "Jan", "Section2", 20 })
            table.Rows.Add(New Object() { "Feb", "Section1", -12 })
            table.Rows.Add(New Object() { "Feb", "Section2", 30 })
            table.Rows.Add(New Object() { "March", "Section1", 14 })
            table.Rows.Add(New Object() { "March", "Section2", 25 })

            Return table
        End Function

        Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.chartControl.Annotations.Clear()
            If chartControl.Diagram.Series(0).Tag.ToString() = "CustomTotal" Then
                Me.chartControl.Diagram.Series.RemoveAt(0)
            End If
            For Each arg As String In Totals.Keys
                Dim ann As New Annotation()
                Dim pos As New RelativePosition() With { _
                    .Angle = 90, _
                    .ConnectorLength = 10 _
                }
                ann.Content = String.Format("{0:N2}", Totals(arg).ActualValue)
                Dim anchor As New PaneAnchorPoint()
                anchor.AxisXCoordinate = New AxisXCoordinate() With {.AxisValue = arg}
                anchor.AxisYCoordinate = New AxisYCoordinate() With {.AxisValue = Totals(arg).DisplayValue}
                ann.ShapePosition = pos
                ann.AnchorPoint = anchor
                Me.chartControl.Annotations.Add(ann)
            Next arg
        End Sub

        Private Sub Button_Click_1(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.chartControl.Annotations.Clear()
            If chartControl.Diagram.Series(0).Tag.ToString() = "CustomTotal" Then
                Me.chartControl.Diagram.Series.RemoveAt(0)
            End If
            Dim totalSeries As New BarSideBySideSeries2D()
            totalSeries.Tag = "CustomTotal"
            For Each arg As String In Totals.Keys
                Dim sp As New SeriesPoint(arg, Totals(arg).DisplayValue)
                sp.ToolTipHint = Totals(arg).ActualValue
                totalSeries.Points.Add(sp)
            Next arg

            totalSeries.LabelsVisibility = True
            totalSeries.Label = New SeriesLabel() With {.TextPattern = "{HINT}"}
            totalSeries.CrosshairLabelVisibility = False
            BarSideBySideSeries2D.SetLabelPosition(totalSeries.Label, Bar2DLabelPosition.Outside)
            Me.chartControl.Diagram.Series.Insert(0, totalSeries)
        End Sub
    End Class

    Public Class PointInfo
        Public Property ActualValue() As Double
        Public Property DisplayValue() As Double
    End Class
End Namespace
