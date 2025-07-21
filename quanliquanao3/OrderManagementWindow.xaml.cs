using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using quanliquanao3.Models;

namespace quanliquanao3
{
    public partial class OrderManagementWindow : Window
    {
        private Managekhoquanao3Context db = new Managekhoquanao3Context();
        public List<Product> ProductsList { get; set; }

        // Dùng để binding dữ liệu nhập kho
        public class ImportItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public OrderManagementWindow()
        {
            InitializeComponent();
            LoadSuppliers();
            LoadProducts();
            dgImportDetails.ItemsSource = new List<ImportItem>();
            LoadImportDetails();

            if (Session.CurrentUser != null)
            {
                txtImportedBy.Text = Session.CurrentUser.FullName;

                // Nếu là nhân viên thì khóa không cho sửa
                if (Session.CurrentUser.Role?.RoleName == "Nhân viên")
                {
                    txtImportedBy.IsReadOnly = true; // hoặc txtImportedBy.IsEnabled = false;
                }
            }
        }

        private void LoadSuppliers()
        {
            cbSuppliers.ItemsSource = db.Suppliers.ToList();
        }

        private void LoadProducts()
        {
            ProductsList = db.Products.ToList();
            var column = dgImportDetails.Columns[0] as DataGridComboBoxColumn;
            column.ItemsSource = ProductsList;
        }

        private void LoadImportDetails()
        {
            var importDetails = db.ImportDetails
                .Include(id => id.Product)
                .Include(id => id.Receipt)
                    .ThenInclude(r => r.Supplier)
                .Where(id => id.Product != null && id.Receipt != null && id.Receipt.Supplier != null)
                .Select(id => new
                {
                    ImportedBy = id.Receipt.ImportedBy,
                    ImportDate = id.Receipt.ImportDate,
                    SupplierName = id.Receipt.Supplier.SupplierName,
                    ProductName = id.Product.ProductName,
                    ProductId = id.Product.ProductId,
                    Quantity = id.Quantity,
                    TotalPrice = id.Quantity * id.Product.Price
                })
                .ToList();

            dgImport.ItemsSource = importDetails;
        }

        private void SaveImport_Click(object sender, RoutedEventArgs e)
        {
            if (cbSuppliers.SelectedValue == null || string.IsNullOrWhiteSpace(txtImportedBy.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin phiếu nhập.");
                return;
            }

            var items = dgImportDetails.ItemsSource as List<ImportItem>;
            if (items == null || !items.Any(i => i.ProductId > 0 && i.Quantity > 0))
            {
                MessageBox.Show("Vui lòng nhập ít nhất một sản phẩm hợp lệ.");
                return;
            }

            var receipt = new ImportReceipt
            {
                ImportDate = dpImportDate.SelectedDate ?? DateTime.Now,
                SupplierId = (int)cbSuppliers.SelectedValue,
                ImportedBy = txtImportedBy.Text
            };

            db.ImportReceipts.Add(receipt);
            db.SaveChanges();

            foreach (var item in items)
            {
                if (item.ProductId == 0 || item.Quantity <= 0) continue;

                var detail = new ImportDetail
                {
                    ReceiptId = receipt.ReceiptId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                db.ImportDetails.Add(detail);

                var product = db.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
            }

            db.SaveChanges();
            MessageBox.Show("Đã lưu phiếu nhập thành công.");
            LoadImportDetails();
            dgImportDetails.ItemsSource = new List<ImportItem>(); // reset
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void cbSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}
