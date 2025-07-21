using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace quanliquanao3
{
    /// <summary>
    /// Interaction logic for EmployeeDashboard.xaml
    /// </summary>
    public partial class EmployeeDashboard : Window
    {
        public EmployeeDashboard()
        {
            InitializeComponent();
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
            exportManagementWindow.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Session.CurrentUser = null;
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
