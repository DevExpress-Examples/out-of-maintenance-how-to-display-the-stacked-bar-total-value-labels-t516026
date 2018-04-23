using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Charts;

namespace StackedTotal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Dictionary<string, PointInfo> Totals { get; set; }
        List<SolidColorBrush> SeriesColors = new List<SolidColorBrush> { Brushes.Yellow, Brushes.Orange, Brushes.LightYellow };

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Totals = new Dictionary<string, PointInfo>();
            this.chartControl.BoundDataChanged += chartControl_BoundDataChanged;
            this.chartControl.Diagram.SeriesDataMember = "Month";
            this.chartControl.Diagram.SeriesTemplate = new BarStackedSeries2D();
            this.chartControl.Diagram.SeriesTemplate.ArgumentDataMember = "Section";
            this.chartControl.Diagram.SeriesTemplate.ValueDataMember = "Value";
            this.chartControl.DataSource = CreateChartData();
        }


        void chartControl_BoundDataChanged(object sender, RoutedEventArgs e)
        {
            Totals.Clear();
            int i = 0;

            foreach (XYSeries2D series in chartControl.Diagram.Series)
            {
                if (series.Tag.ToString() != "CustomTotal")
                {
                    series.Brush = SeriesColors[i++];
                    foreach (SeriesPoint sp in series.Points)
                    {
                        if (!Totals.ContainsKey(sp.Argument))
                            Totals.Add(sp.Argument, new PointInfo() { ActualValue = sp.Value, DisplayValue = sp.Value > 0? sp.Value: 0});
                        else
                        {
                            PointInfo info = Totals[sp.Argument];
                            info.ActualValue += sp.Value; 
                            if (sp.Value > 0)
                            info.DisplayValue += sp.Value;
                            Totals[sp.Argument] = info;
                        }
                    }
                }
            }
        }

        private DataTable CreateChartData()
        {

            DataTable table = new DataTable("Table1");

            table.Columns.Add("Month", typeof(String));
            table.Columns.Add("Section", typeof(String));
            table.Columns.Add("Value", typeof(Int32));

            table.Rows.Add(new object[] { "Jan", "Section1", 10 });
            table.Rows.Add(new object[] { "Jan", "Section2", 20 });
            table.Rows.Add(new object[] { "Feb", "Section1", -12 });
            table.Rows.Add(new object[] { "Feb", "Section2", 30 });
            table.Rows.Add(new object[] { "March", "Section1", 14 });
            table.Rows.Add(new object[] { "March", "Section2", 25 });

            return table;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.chartControl.Annotations.Clear();
            if (chartControl.Diagram.Series[0].Tag.ToString() == "CustomTotal")
                this.chartControl.Diagram.Series.RemoveAt(0);
            foreach (string arg in Totals.Keys)
            {
                Annotation ann = new Annotation();
                RelativePosition pos = new RelativePosition() { Angle = 90, ConnectorLength = 10 };
                ann.Content = string.Format("{0:N2}", Totals[arg].ActualValue);
                PaneAnchorPoint anchor = new PaneAnchorPoint();
                anchor.AxisXCoordinate = new AxisXCoordinate() { AxisValue = arg };
                anchor.AxisYCoordinate = new AxisYCoordinate() { AxisValue = Totals[arg].DisplayValue };
                ann.ShapePosition = pos;
                ann.AnchorPoint = anchor;
                this.chartControl.Annotations.Add(ann);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.chartControl.Annotations.Clear();
            if (chartControl.Diagram.Series[0].Tag.ToString() == "CustomTotal")
                this.chartControl.Diagram.Series.RemoveAt(0);
            BarSideBySideSeries2D totalSeries = new BarSideBySideSeries2D();
            totalSeries.Tag = "CustomTotal";
            foreach (string arg in Totals.Keys)
            {
                SeriesPoint sp = new SeriesPoint(arg, Totals[arg].DisplayValue);
                sp.ToolTipHint = Totals[arg].ActualValue;
                totalSeries.Points.Add(sp);
            }

            totalSeries.LabelsVisibility = true;
            totalSeries.Label = new SeriesLabel() { TextPattern = "{HINT}" };
            totalSeries.CrosshairLabelVisibility = false;
            BarSideBySideSeries2D.SetLabelPosition(totalSeries.Label, Bar2DLabelPosition.Outside);
            this.chartControl.Diagram.Series.Insert(0, totalSeries);
        }
    }

    public class PointInfo
    {
        public double ActualValue { get; set; }
        public double DisplayValue { get; set; }
    }
}
