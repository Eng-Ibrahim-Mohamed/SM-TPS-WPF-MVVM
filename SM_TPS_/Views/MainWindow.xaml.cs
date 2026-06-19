using System.Windows;
using System.Diagnostics;
using System.IO;
namespace SM_TPS_.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // مكانها الشرعي الصحيح جوه الشاشة ✅
            InitializeComponent();

        }
        private void SalesHistory_Click(object sender, RoutedEventArgs e)
        {
            SalesHistoryWindow window =
                new SalesHistoryWindow();

            window.ShowDialog();
        }
        private void ViewInvoice_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "Invoice.pdf";

            if (File.Exists(filePath))
            {
                Process.Start(filePath);
            }
            else
            {
                MessageBox.Show(
                    "No Invoice Found",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
        private void PrintInvoice_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "Invoice.pdf";

            if (!File.Exists(filePath))
            {
                MessageBox.Show(
                    "No Invoice Found",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            ProcessStartInfo info =
                new ProcessStartInfo(filePath);

            info.Verb = "print";
            info.CreateNoWindow = true;
            info.WindowStyle =
                ProcessWindowStyle.Hidden;

            Process.Start(info);
        }
        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            UserManagementWindow window =
                new UserManagementWindow();

            window.ShowDialog();
        }
    }
}