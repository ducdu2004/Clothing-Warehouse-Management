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

namespace quanliquanao3
{
    /// <summary>
    /// Interaction logic for HistoryManagementWindow.xaml
    /// </summary>
    public partial class HistoryManagementWindow : Window
    {

        public class TransactionHistory
        {
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public DateTime Date { get; set; }
            public string TransactionType { get; set; } // "Nhập" hoặc "Xuất"

            public decimal Total { get; set; } // Tổng tiền
        }

        private Managekhoquanao3Context db = new Managekhoquanao3Context();
        public HistoryManagementWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            cbProduct.ItemsSource = db.Products.ToList();
        }
        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(cbFilterTime.SelectedItem is ComboBoxItem selectedItem)) return;

            string filter = selectedItem.Content.ToString();
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Now;

            switch (filter)
            {
                case "Tuần":
                    startDate = DateTime.Today.AddDays(-7);
                    break;
                case "Tháng":
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    break;
                case "Năm":
                    startDate = new DateTime(DateTime.Today.Year, 1, 1);
                    break;
            }

            // Giao dịch nhập
            var importData = db.ImportDetails
                .Include(i => i.Product)
                .Include(i => i.Receipt)
                .Where(i => i.Receipt.ImportDate >= startDate)
                .Select(i => new TransactionHistory
                {
                    ItemCode = i.Product.ProductId.ToString(),
                    ItemName = i.Product.ProductName,
                    Quantity = (int)i.Quantity,
                    Date = i.Receipt.ImportDate,
                    TransactionType = "Nhập"
                });

            // Giao dịch xuất
            var exportData = db.ExportDetails
                .Include(e => e.Product)
                .Include(e => e.Receipt)
                .Where(e => e.Receipt.ExportDate >= startDate)
                .Select(e => new TransactionHistory
                {
                    ItemCode = e.Product.ProductId.ToString(),
                    ItemName = e.Product.ProductName,
                    Quantity = (int)e.Quantity,
                    Date = e.Receipt.ExportDate,
                    TransactionType = "Xuất"
                });

            // Gộp lại và hiển thị
            var all = importData.Concat(exportData)
                                .OrderByDescending(t => t.Date)
                                .ToList();

            dgHistory.ItemsSource = all;
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!(cbFilterTime.SelectedItem is ComboBoxItem timeItem)) return;
            if (!(cbTransactionType.SelectedItem is ComboBoxItem typeItem)) return;

            string timeFilter = timeItem.Content.ToString();
            string typeFilter = typeItem.Content.ToString();
            int? selectedProductId = cbProduct.SelectedValue as int?;

            DateTime startDate = DateTime.Today;
            switch (timeFilter)
            {
                case "Tuần":
                    startDate = DateTime.Today.AddDays(-7);
                    break;
                case "Tháng":
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    break;
                case "Năm":
                    startDate = new DateTime(DateTime.Today.Year, 1, 1);
                    break;
            }

            var importData = db.ImportDetails
                .Include(i => i.Product)
                .Include(i => i.Receipt)
                .Where(i => i.Receipt.ImportDate >= startDate)
                .Select(i => new TransactionHistory
                {
                    ItemCode = i.Product.ProductId.ToString(),
                    ItemName = i.Product.ProductName,
                    Quantity = (int)i.Quantity,
                    Date = i.Receipt.ImportDate,
                    TransactionType = "Nhập",
                    Total = ((int)i.Quantity) * ((decimal)i.Product.Price)
                });

            var exportData = db.ExportDetails
                .Include(e => e.Product)
                .Include(e => e.Receipt)
                .Where(e => e.Receipt.ExportDate >= startDate)
                .Select(e => new TransactionHistory
                {
                    ItemCode = e.Product.ProductId.ToString(),
                    ItemName = e.Product.ProductName,
                    Quantity = (int)e.Quantity,
                    Date = e.Receipt.ExportDate,
                    TransactionType = "Xuất",
                    Total = ((int)e.Quantity) * ((decimal)e.Product.Price * 1.05m)
                });

            var all = importData.Concat(exportData).ToList();

            // Lọc theo loại giao dịch
            if (typeFilter == "Nhập")
                all = all.Where(t => t.TransactionType == "Nhập").ToList();
            else if (typeFilter == "Xuất")
                all = all.Where(t => t.TransactionType == "Xuất").ToList();

            // Lọc theo sản phẩm
            if (selectedProductId.HasValue)
                all = all.Where(t => t.ItemCode == selectedProductId.Value.ToString()).ToList();

            // Gán DataGrid
            dgHistory.ItemsSource = all;

            // Tính tổng tiền
            // Tổng nhập
            decimal totalImport = all
                .Where(t => t.TransactionType == "Nhập")
                .Sum(t => t.Total);

            // Tổng xuất
            decimal totalExport = all
                .Where(t => t.TransactionType == "Xuất")
                .Sum(t => t.Total);

            // Doanh thu = Xuất - Nhập
            decimal revenue = totalExport - totalImport;
            txtTotal.Text = revenue.ToString("N0") + " VNĐ";
        }
    }

}

