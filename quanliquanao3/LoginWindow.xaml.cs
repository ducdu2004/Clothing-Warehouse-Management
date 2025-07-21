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
using Microsoft.EntityFrameworkCore;
using quanliquanao3.Models;
using static System.Collections.Specialized.BitVector32;

namespace quanliquanao3
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        Managekhoquanao3Context db = new Managekhoquanao3Context();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            var user = db.Users
                         .Include(u => u.Role)
                         .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                Session.CurrentUser = user;

                MessageBox.Show($"Xin chào {user.FullName} ({user.Role.RoleName})", "Đăng nhập thành công");

                  if (user.Role.RoleId == 2)
                {
                    var emp = new EmployeeDashboard();
                    emp.Show();
                }
                else if (user.Role.RoleId == 1)
                {
                    var main = new MainWindow();
                    main.Show();

                }
               
                this.Close();

            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
