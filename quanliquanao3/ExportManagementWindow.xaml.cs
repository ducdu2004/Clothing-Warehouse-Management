using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using quanliquanao3.Models;

namespace quanliquanao3
{
    public partial class ExportManagementWindow : Window
    {
        Managekhoquanao3Context db = new Managekhoquanao3Context();
        public List<Product> ProductsList { get; set; }

        // Class tạm cho binding DataGrid xuất kho
        public class ExportItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public ExportManagementWindow()
        {
            InitializeComponent();
            LoadCustomers();
            LoadProducts();
            LoadExportDetails();
            dgExportDetails.ItemsSource = new List<ExportItem>();
        }

        private void LoadCustomers()
        {
            cbCustomers.ItemsSource = db.Customers.ToList();
        }

        private void LoadProducts()
        {
            ProductsList = db.Products.ToList();
            var column = dgExportDetails.Columns[0] as DataGridComboBoxColumn;
            column.ItemsSource = ProductsList;
        }

        private void LoadExportDetails()
        {
            var exportDetails = db.ExportDetails
                .Include(ed => ed.Product)
                .Include(ed => ed.Receipt)
                    .ThenInclude(r => r.Customer)
                .Where(ed => ed.Product != null && ed.Receipt != null && ed.Receipt.Customer != null)
                .Select(ed => new
                {
                    ExportDate = ed.Receipt.ExportDate,
                    CustomerName = ed.Receipt.Customer.CustomerName,
                    ProductName = ed.Product.ProductName,
                    Quantity = ed.Quantity,
                    TotalPrice = ed.Quantity * (ed.Product.Price * 1.05m) // +5%
                })
                .ToList();

            dgExport.ItemsSource = exportDetails;
        }

        private void SaveExport_Click(object sender, RoutedEventArgs e)
        {
            if (cbCustomers.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng.");
                return;
            }

            var items = dgExportDetails.ItemsSource as List<ExportItem>;
            if (items == null || items.Count == 0 || items.All(i => i.Quantity <= 0))
            {
                MessageBox.Show("Vui lòng nhập ít nhất một sản phẩm hợp lệ.");
                return;
            }

            var selectedCustomer = cbCustomers.SelectedItem as Customer;

            var receipt = new ExportReceipt
            {
                CustomerId = selectedCustomer.CustomerId,
                ExportDate = dpImportDate.SelectedDate ?? DateTime.Now,
                ExportedBy = "Nhân viên A", // TODO: có thể thay bằng User đang đăng nhập
                Note = "Xuất hàng"
            };

            db.ExportReceipts.Add(receipt);
            db.SaveChanges();

            foreach (var item in items)
            {
                if (item.ProductId == 0 || item.Quantity <= 0) continue;

                var exportDetail = new ExportDetail
                {
                    ReceiptId = receipt.ReceiptId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                db.ExportDetails.Add(exportDetail);

                // Trừ tồn kho
                var product = db.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                }
            }

            db.SaveChanges();
            MessageBox.Show("Xuất kho thành công.");
            LoadExportDetails();
            dgExportDetails.ItemsSource = new List<ExportItem>(); // reset form
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void cbCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Có thể load thông tin chi tiết khách nếu cần
        }
    }
}
