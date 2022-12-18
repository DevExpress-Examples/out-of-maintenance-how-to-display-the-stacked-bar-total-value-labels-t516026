Imports System.Data
Imports System.Windows
Imports System.Windows.Controls
Imports DevExpress.Xpf.Charts

Namespace StackedTotal

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            Me.InitializeComponent()
        End Sub

        Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.chartControl.Diagram.SeriesDataMember = "Month"
            Me.chartControl.Diagram.SeriesTemplate = New BarStackedSeries2D()
            Me.chartControl.Diagram.SeriesTemplate.ArgumentDataMember = "Section"
            Me.chartControl.Diagram.SeriesTemplate.ValueDataMember = "Value"
            Me.chartControl.DataSource = CreateChartData()
        End Sub

        Private Function CreateChartData() As DataTable
            Dim table As DataTable = New DataTable("Table1")
            table.Columns.Add("Month", GetType(String))
            table.Columns.Add("Section", GetType(String))
            table.Columns.Add("Value", GetType(Integer))
            table.Rows.Add(New Object() {"Jan", "Section1", 10})
            table.Rows.Add(New Object() {"Jan", "Section2", 20})
            table.Rows.Add(New Object() {"Feb", "Section1", -12})
            table.Rows.Add(New Object() {"Feb", "Section2", 30})
            table.Rows.Add(New Object() {"March", "Section1", 14})
            table.Rows.Add(New Object() {"March", "Section2", 25})
            Return table
        End Function
    End Class
End Namespace
