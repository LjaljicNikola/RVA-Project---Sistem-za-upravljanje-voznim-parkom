using System.IO;
using System.Windows;

namespace Component1.InformationSystem.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RefreshLog()
        {
            if (File.Exists("activity_log.txt"))
                LogTextBox.Text = File.ReadAllText("activity_log.txt");
        }
    }
}
