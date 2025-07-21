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
using quanliquanao3.Models;

namespace quanliquanao3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Managekhoquanao3Context db = new Managekhoquanao3Context();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ProductManagementButton_Click(object sender, RoutedEventArgs e)
        {
            var productManagementWindow = new ProductManagementWindow();
            productManagementWindow.Show();
            this.Close();
        }

        private void OrderManagementButton_Click(object sender, RoutedEventArgs e)
        {
            var orderManagementWindow = new OrderManagementWindow();
            orderManagementWindow.Show();
            this.Close();
        }

        private void ExportManageButton_Click(object sender, RoutedEventArgs e)
        {
            var exportManagementWindow = new ExportManagementWindow();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Session.CurrentUser = null;
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void HistoryManagementButton_Click(object sender, RoutedEventArgs e)
        {
            var historyManagementWindow = new HistoryManagementWindow();
            historyManagementWindow.Show();
            this.Close();
        }
    }
}