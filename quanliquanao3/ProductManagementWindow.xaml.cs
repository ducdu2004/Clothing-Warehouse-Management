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
using CourseManagemanet;
using Microsoft.EntityFrameworkCore;
using quanliquanao3.Models;

namespace quanliquanao3
{
    /// <summary>
    /// Interaction logic for ProductManagementWindow.xaml
    /// </summary>
    public partial class ProductManagementWindow : Window
    {
        Managekhoquanao3Context db = new Managekhoquanao3Context();
        private bool isEditing = false;
        public ProductManagementWindow()
        {
            InitializeComponent();
            LoadInitialDataAndComboBoxes();

        }

        private void LoadInitialDataAndComboBoxes()
        {
            dgProduct.ItemsSource = db.Products.Include(p => p.Category).ToList();

            cbCategory.ItemsSource = db.ProductCategories.ToList();

            cbCategoryInput.ItemsSource = db.ProductCategories.ToList();

            cbSize.ItemsSource = db.Products.Select(p => p.Size).Distinct().ToList();
        }

        private void ApplyFilters()
        {
            int? selectedCategoryId = cbCategory.SelectedValue as int?;

            int? selectedCategoryInputId = cbCategoryInput.SelectedValue as int?;

            string selectedSize = cbSize.SelectedItem as string;

            var query = db.Products.Include(p => p.Category).AsQueryable();

            if (selectedCategoryId.HasValue && selectedCategoryId != 0)
            {
                query = query.Where(p => p.CategoryId == selectedCategoryId.Value);
            }

            if (selectedCategoryInputId.HasValue && selectedCategoryInputId != 0)
            {
                query = query.Where(p => p.CategoryId == selectedCategoryInputId.Value);
            }

            if (!string.IsNullOrEmpty(selectedSize))
            {
                query = query.Where(p => p.Size == selectedSize);
            }

            dgProduct.ItemsSource = query.ToList();
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void EnableInputs(bool isEnabled, bool isAddMode = false)
        {
            txtProductId.IsEnabled = isEnabled && isAddMode; // Chỉ cho phép chỉnh sửa ProductId khi ở chế độ thêm mới
            txtProductName.IsEnabled = isEnabled;
            txtColor.IsEnabled = isEnabled;
            txtPrice.IsEnabled = isEnabled;
            txtQuantity.IsEnabled = isEnabled;
            cbCategoryInput.IsEnabled = isEnabled;
            btnSave.IsEnabled = isEnabled;
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            isEditing = false;
            EnableInputs(true, isAddMode: true);
            txtProductId.Text = ""; // Xóa trường ProductId khi thêm mới
            txtProductName.Text = "";
            cbCategoryInput.SelectedIndex = -1;
            txtColor.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
            txtProductName.Focus(); // Đặt focus vào trường Product Name
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            EnableInputs(true);
            var selectProduct = dgProduct.SelectedItem as Product;
            if (selectProduct == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên để sửa.");
                return;
            }
            isEditing = true;
            EnableInputs(true, isAddMode: false);
            txtProductId.Text = selectProduct.ProductId.ToString();
            txtProductName.Text = selectProduct.ProductName;
            txtColor.Text = selectProduct.Color;
            txtPrice.Text = selectProduct.Price.ToString();
            txtQuantity.Text = selectProduct.Quantity.ToString();
            cbCategoryInput.SelectedValue = selectProduct.CategoryId;
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            var selectedProduct = dgProduct.SelectedItem as Product;
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a student to delete.");
                return;
            }
            var oldStudent = selectedProduct;
            if (MessageBox.Show($"Are you sure you want to delete student {oldStudent.ProductName}?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.ExportDetails
                    .Where(rb => rb.ProductId == oldStudent.ProductId)
                    .ToList()
                    .ForEach(rb => db.ExportDetails.Remove(rb));
                db.ImportDetails
                    .Where(sc => sc.ProductId == oldStudent.ProductId)
                    .ToList()
                    .ForEach(sc => db.ImportDetails.Remove(sc));
                db.Products.Remove(oldStudent);
                db.SaveChanges();
                MessageBox.Show("Student deleted successfully.");
                LoadInitialDataAndComboBoxes();
            }
            else
            {
                MessageBox.Show("Delete operation cancelled.");
            }
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            cbCategory.SelectedIndex = -1;
            cbSize.SelectedIndex = -1;
            ApplyFilters();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string productStr = txtProductId.Text.Trim();
            string productName = txtProductName.Text.Trim();
            string color = txtColor.Text.Trim();
            string priceStr = txtPrice.Text.Trim();
            string quantityStr = txtQuantity.Text.Trim();
            int? categoryId = cbCategoryInput.SelectedValue as int?;


            string errorMessage;

            if (!ValidationHelper.IsValidPersonName(productName, "Product Name", out errorMessage))
            {
                MessageBox.Show(errorMessage);
                return;
            }

            if (!ValidationHelper.IsValidPersonName(color, "Color", out errorMessage))
            {
                MessageBox.Show(errorMessage);
                return;
            }
            if (!ValidationHelper.IsPositiveInteger(quantityStr, "Quantity", out int Quantity, out errorMessage))
            {
                MessageBox.Show(errorMessage);
                return;
            }
            if (!ValidationHelper.IsPositiveDecimal(priceStr, "Price", out decimal Price, out errorMessage))
            {
                MessageBox.Show(errorMessage);
                return;
            }

            if (isEditing)
            {

                if (!ValidationHelper.IsPositiveInteger(productStr, "StudentId", out int ProductId, out errorMessage))
                {
                    MessageBox.Show(errorMessage);
                    return;
                }

                var product = db.Products.FirstOrDefault(s => s.ProductId == ProductId);
                if (product == null)
                {
                    MessageBox.Show("Không tìm thấy sinh viên để cập nhật.");
                    return;
                }
                product.ProductName = productName;
                product.Color = color;
                product.CategoryId = categoryId;
                product.Quantity = Quantity;
                product.Price = Price;

                db.SaveChanges();
                MessageBox.Show("Cập nhật sinh viên thành công.");

            }
            else
            {
                var newProduct = new Product
                {
                    ProductName = productName,
                    Color = color,
                    CategoryId = categoryId,
                    Quantity = Quantity,
                    Price = Price
                };


                db.Products.Add(newProduct);
                db.SaveChanges();
                MessageBox.Show("Thêm sản phẩm thành công.");
            }

            LoadInitialDataAndComboBoxes();
            EnableInputs(false);
        }

        private void cbCategoryInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbCategoryInput.ItemsSource = db.ProductCategories.ToList();

        }
    }
}
