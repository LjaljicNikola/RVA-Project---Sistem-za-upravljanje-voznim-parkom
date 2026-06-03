using System.Windows;
using Component2.Analytics.Services;
using Component2.Analytics.ViewModels;

namespace Component2.Analytics.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var adapter     = new WcfFleetDataAdapter();
            var csvExporter = new CsvExportService();
            DataContext = new AnalyticsViewModel(adapter, csvExporter);
        }
    }
}
