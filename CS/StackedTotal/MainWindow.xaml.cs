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

    
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            this.chartControl.Diagram.SeriesDataMember = "Month";
            this.chartControl.Diagram.SeriesTemplate = new BarStackedSeries2D();
            this.chartControl.Diagram.SeriesTemplate.ArgumentDataMember = "Section";
            this.chartControl.Diagram.SeriesTemplate.ValueDataMember = "Value";
            this.chartControl.DataSource = CreateChartData();
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


    }

  
}
