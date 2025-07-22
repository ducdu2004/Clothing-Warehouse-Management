using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient; // Thư viện để kết nối với SQL Server
using System.Linq; // Để sử dụng .Any() cho List
using System.Text; // Để sử dụng Encoding.UTF8

namespace CRUDquanao
{
    // Lớp Product đại diện cho một sản phẩm với các trường
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; } // Thêm trường Status (true = 1, false = 0)
    }

    // Lớp ProductCategory đại diện cho một danh mục sản phẩm
    public class ProductCategory
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

    // Lớp DatabaseHelper để xử lý các tương tác với SQL Server
    public class DatabaseHelper
    {
        // Chuỗi kết nối đến cơ sở dữ liệu SQL Server của bạn
        // Đã thêm TrustServerCertificate=True để bỏ qua lỗi chứng chỉ SSL
        private readonly string connectionString = "Server=localhost;Database=managekhoquanao3;Trusted_Connection=True;TrustServerCertificate=True;";

        // Phương thức để thêm một sản phẩm mới vào cơ sở dữ liệu
        public void AddProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Câu lệnh SQL để chèn dữ liệu vào bảng Products với tất cả các cột, bao gồm Status
                    string query = "INSERT INTO Products (ProductName, CategoryID, Size, Color, Price, Quantity, Status) VALUES (@ProductName, @CategoryID, @Size, @Color, @Price, @Quantity, @Status)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                        command.Parameters.AddWithValue("@Size", product.Size);
                        command.Parameters.AddWithValue("@Color", product.Color);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Quantity", product.Quantity);
                        command.Parameters.AddWithValue("@Status", product.Status); // Thêm Status

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Sản phẩm '{product.ProductName}' đã được thêm thành công.");
                        }
                        else
                        {
                            Console.WriteLine("Không thể thêm sản phẩm.");
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Lỗi khi thêm sản phẩm: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Đã xảy ra lỗi không mong muốn: {ex.Message}");
                }
            }
        }

        // Phương thức để lấy tất cả sản phẩm từ cơ sở dữ liệu
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Câu lệnh SQL để chọn tất cả sản phẩm với các trường, bao gồm Status
                    string query = "SELECT ProductID, ProductName, CategoryID, Size, Color, Price, Quantity, Status FROM Products";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                    Size = reader.GetString(reader.GetOrdinal("Size")),
                                    Color = reader.GetString(reader.GetOrdinal("Color")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    Status = reader.GetBoolean(reader.GetOrdinal("Status")) // Lấy Status
                                });
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Đã xảy ra lỗi không mong muốn: {ex.Message}");
                }
            }
            return products;
        }

        // Phương thức để lấy một sản phẩm theo ProductID
        public Product GetProductById(int id)
        {
            Product product = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ProductID, ProductName, CategoryID, Size, Color, Price, Quantity, Status FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", id);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product = new Product
                                {
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                    Size = reader.GetString(reader.GetOrdinal("Size")),
                                    Color = reader.GetString(reader.GetOrdinal("Color")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    Status = reader.GetBoolean(reader.GetOrdinal("Status"))
                                };
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi khi lấy sản phẩm theo ID: {ex.Message}");
                    }
                }
            }
            return product;
        }

        // Phương thức để cập nhật thông tin sản phẩm
        public void UpdateProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Products SET ProductName = @ProductName, CategoryID = @CategoryID, Size = @Size, Color = @Color, Price = @Price, Quantity = @Quantity, Status = @Status WHERE ProductID = @ProductID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                    command.Parameters.AddWithValue("@Size", product.Size);
                    command.Parameters.AddWithValue("@Color", product.Color);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Quantity", product.Quantity);
                    command.Parameters.AddWithValue("@Status", product.Status); // Cập nhật Status
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Sản phẩm có ProductID {product.ProductID} đã được cập nhật thành công.");
                        }
                        else
                        {
                            Console.WriteLine($"Không tìm thấy sản phẩm có ProductID {product.ProductID} để cập nhật hoặc không có thay đổi.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi khi cập nhật sản phẩm: {ex.Message}");
                    }
                }
            }
        }

        // Phương thức để cập nhật trạng thái sản phẩm (Status)
        public void UpdateProductStatus(int productId, bool status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Products SET Status = @Status WHERE ProductID = @ProductID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@ProductID", productId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Trạng thái sản phẩm có ProductID {productId} đã được cập nhật thành công thành {(status ? "Hoạt động" : "Không hoạt động")}.");
                        }
                        else
                        {
                            Console.WriteLine($"Không tìm thấy sản phẩm có ProductID {productId} để cập nhật trạng thái.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi khi cập nhật trạng thái sản phẩm: {ex.Message}");
                    }
                }
            }
        }

        // Phương thức để xóa một sản phẩm khỏi cơ sở dữ liệu
        public void DeleteProductFromDatabase(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", id);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Sản phẩm có ProductID {id} đã được xóa hoàn toàn khỏi cơ sở dữ liệu.");
                        }
                        else
                        {
                            Console.WriteLine($"Không tìm thấy sản phẩm có ProductID {id} để xóa.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi khi xóa sản phẩm: {ex.Message}");
                    }
                }
            }
        }

        // Phương thức kiểm tra xem CategoryID có tồn tại không
        public bool CategoryExists(int categoryId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM ProductCategories WHERE CategoryID = @CategoryID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryID", categoryId);
                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi khi kiểm tra CategoryID: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        // Phương thức kiểm tra xem sản phẩm có chi tiết nhập kho không
        public bool HasImportDetails(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM ImportDetails WHERE ProductID = @ProductID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi khi kiểm tra chi tiết nhập kho: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        // Phương thức kiểm tra xem sản phẩm có chi tiết xuất kho không
        public bool HasExportDetails(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM ExportDetails WHERE ProductID = @ProductID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi khi kiểm tra chi tiết xuất kho: {ex.Message}");
                        return false;
                    }
                }
            }
        }
    }

    // Lớp ValidationHelper để xử lý các logic kiểm tra dữ liệu
    public static class ValidationHelper
    {
        // Phương thức kiểm tra và chuyển đổi chuỗi thành số nguyên dương
        public static bool TryParsePositiveInt(string input, out int result, string fieldName)
        {
            if (!int.TryParse(input, out result) || result <= 0)
            {
                Console.WriteLine($"{fieldName} không hợp lệ. Vui lòng nhập một số nguyên dương.");
                return false;
            }
            return true;
        }

        // Phương thức kiểm tra và chuyển đổi chuỗi thành số nguyên không âm
        public static bool TryParseNonNegativeInt(string input, out int result, string fieldName)
        {
            if (!int.TryParse(input, out result) || result < 0)
            {
                Console.WriteLine($"{fieldName} không hợp lệ. Vui lòng nhập một số nguyên không âm.");
                return false;
            }
            return true;
        }

        // Phương thức kiểm tra và chuyển đổi chuỗi thành số thập phân dương
        public static bool TryParsePositiveDecimal(string input, out decimal result, string fieldName)
        {
            if (!decimal.TryParse(input, out result) || result <= 0)
            {
                Console.WriteLine($"{fieldName} không hợp lệ. Vui lòng nhập một số dương.");
                return false;
            }
            return true;
        }
    }

    // Lớp ConsoleProductManager để quản lý menu và tương tác người dùng
    public class ConsoleProductManager
    {
        private DatabaseHelper _dbHelper;

        public ConsoleProductManager()
        {
            _dbHelper = new DatabaseHelper();
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Đảm bảo hiển thị tiếng Việt đúng cách
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllProducts();
                        break;
                    case "2":
                        AddNewProduct();
                        break;
                    case "3":
                        UpdateExistingProduct();
                        break;
                    case "4":
                        DeleteProduct();
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("Đang thoát ứng dụng. Tạm biệt!");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                        break;
                }
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("\n--- ỨNG DỤNG QUẢN LÝ SẢN PHẨM ---");
            Console.WriteLine("1. Xem danh sách sản phẩm");
            Console.WriteLine("2. Thêm sản phẩm mới");
            Console.WriteLine("3. Cập nhật sản phẩm");
            Console.WriteLine("4. Xóa sản phẩm");
            Console.WriteLine("5. Thoát");
            Console.Write("Chọn một tùy chọn: ");
        }

        private void ViewAllProducts()
        {
            List<Product> products = _dbHelper.GetAllProducts();
            if (products.Any())
            {
                Console.WriteLine("\n--- DANH SÁCH SẢN PHẨM ---");
                Console.WriteLine("{0,-5} {1,-25} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10} {7,-10}", "ID", "Tên SP", "Cat ID", "Size", "Color", "Giá", "SL", "Trạng thái");
                Console.WriteLine("--------------------------------------------------------------------------------------------------");
                foreach (var product in products)
                {
                    Console.WriteLine("{0,-5} {1,-25} {2,-10} {3,-10} {4,-10} {5,-10:N0} {6,-10} {7,-10}",
                        product.ProductID, product.ProductName, product.CategoryID, product.Size, product.Color, product.Price, product.Quantity, (product.Status ? "Hoạt động" : "Không"));
                }
            }
            else
            {
                Console.WriteLine("Không có sản phẩm nào trong cơ sở dữ liệu.");
            }
        }

        private void AddNewProduct()
        {
            Console.Write("Nhập tên sản phẩm: ");
            string productName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(productName))
            {
                Console.WriteLine("Tên sản phẩm không được để trống.");
                return;
            }

            Console.Write("Nhập Category ID: ");
            int categoryID;
            string categoryIDInput = Console.ReadLine();
            if (!ValidationHelper.TryParsePositiveInt(categoryIDInput, out categoryID, "Category ID"))
            {
                return;
            }

            // Kiểm tra sự tồn tại của CategoryID
            if (!_dbHelper.CategoryExists(categoryID))
            {
                Console.WriteLine($"Lỗi: Category ID '{categoryID}' không tồn tại. Vui lòng nhập một Category ID hợp lệ.");
                return;
            }

            Console.Write("Nhập kích thước (Size): ");
            string size = Console.ReadLine();
            Console.Write("Nhập màu sắc (Color): ");
            string color = Console.ReadLine();

            Console.Write("Nhập giá sản phẩm: ");
            decimal price;
            string priceInput = Console.ReadLine();
            if (!ValidationHelper.TryParsePositiveDecimal(priceInput, out price, "Giá sản phẩm"))
            {
                return;
            }

            Console.Write("Nhập số lượng (Quantity): ");
            int quantity;
            string quantityInput = Console.ReadLine();
            if (!ValidationHelper.TryParseNonNegativeInt(quantityInput, out quantity, "Số lượng"))
            {
                return;
            }

            _dbHelper.AddProduct(new Product
            {
                ProductName = productName,
                CategoryID = categoryID,
                Size = size,
                Color = color,
                Price = price,
                Quantity = quantity,
                Status = true // Mặc định sản phẩm mới là hoạt động
            });
        }

        private void UpdateExistingProduct()
        {
            Console.Write("Nhập Product ID sản phẩm cần cập nhật: ");
            int updateId;
            string updateIdInput = Console.ReadLine();
            if (!ValidationHelper.TryParsePositiveInt(updateIdInput, out updateId, "Product ID"))
            {
                return;
            }

            Product productToUpdate = _dbHelper.GetProductById(updateId);
            if (productToUpdate != null)
            {
                Console.WriteLine($"\n--- CẬP NHẬT SẢN PHẨM ID: {productToUpdate.ProductID} ---");
                Console.WriteLine($"Tên hiện tại: {productToUpdate.ProductName}");
                Console.Write("Nhập tên mới (để trống nếu không đổi): ");
                string newProductName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newProductName))
                {
                    productToUpdate.ProductName = newProductName;
                }

                Console.WriteLine($"Category ID hiện tại: {productToUpdate.CategoryID}");
                Console.Write("Nhập Category ID mới (để trống nếu không đổi): ");
                string newCategoryIDInput = Console.ReadLine();
                int newCategoryID;
                if (!string.IsNullOrWhiteSpace(newCategoryIDInput))
                {
                    if (ValidationHelper.TryParsePositiveInt(newCategoryIDInput, out newCategoryID, "Category ID mới"))
                    {
                        if (_dbHelper.CategoryExists(newCategoryID))
                        {
                            productToUpdate.CategoryID = newCategoryID;
                        }
                        else
                        {
                            Console.WriteLine($"Lỗi: Category ID '{newCategoryID}' không tồn tại. Giữ nguyên Category ID cũ.");
                        }
                    }
                }

                Console.WriteLine($"Kích thước hiện tại: {productToUpdate.Size}");
                Console.Write("Nhập kích thước mới (để trống nếu không đổi): ");
                string newSize = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newSize))
                {
                    productToUpdate.Size = newSize;
                }

                Console.WriteLine($"Màu sắc hiện tại: {productToUpdate.Color}");
                Console.Write("Nhập màu sắc mới (để trống nếu không đổi): ");
                string newColor = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newColor))
                {
                    productToUpdate.Color = newColor;
                }

                Console.WriteLine($"Giá hiện tại: {productToUpdate.Price}");
                Console.Write("Nhập giá mới (để trống nếu không đổi): ");
                string newPriceInput = Console.ReadLine();
                decimal newPrice;
                if (!string.IsNullOrWhiteSpace(newPriceInput))
                {
                    if (ValidationHelper.TryParsePositiveDecimal(newPriceInput, out newPrice, "Giá mới"))
                    {
                        productToUpdate.Price = newPrice;
                    }
                }

                Console.WriteLine($"Số lượng hiện tại: {productToUpdate.Quantity}");
                Console.Write("Nhập số lượng mới (để trống nếu không đổi): ");
                string newQuantityInput = Console.ReadLine();
                int newQuantity;
                if (!string.IsNullOrWhiteSpace(newQuantityInput))
                {
                    if (ValidationHelper.TryParseNonNegativeInt(newQuantityInput, out newQuantity, "Số lượng mới"))
                    {
                        productToUpdate.Quantity = newQuantity;
                    }
                }

                // Kiểm tra xem sản phẩm đã được nhập/xuất chưa
                bool hasImported = _dbHelper.HasImportDetails(productToUpdate.ProductID);
                bool hasExported = _dbHelper.HasExportDetails(productToUpdate.ProductID);

                if (hasImported || hasExported)
                {
                    Console.WriteLine("Sản phẩm này đã có chi tiết nhập/xuất kho. Bạn không thể thay đổi trạng thái hoạt động trực tiếp.");
                    // Nếu sản phẩm đã có giao dịch, chỉ cho phép cập nhật thông tin khác, không thay đổi Status từ đây.
                    // Nếu muốn thay đổi Status, phải dùng chức năng riêng (ví dụ: "Ngừng kinh doanh")
                }
                else
                {
                    Console.WriteLine($"Trạng thái hiện tại: {(productToUpdate.Status ? "Hoạt động" : "Không hoạt động")}");
                    Console.Write("Cập nhật trạng thái (true/false, để trống nếu không đổi): ");
                    string newStatusInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newStatusInput))
                    {
                        if (bool.TryParse(newStatusInput, out bool newStatus))
                        {
                            productToUpdate.Status = newStatus;
                        }
                        else
                        {
                            Console.WriteLine("Trạng thái không hợp lệ. Giữ nguyên trạng thái cũ.");
                        }
                    }
                }

                _dbHelper.UpdateProduct(productToUpdate);
            }
            else
            {
                Console.WriteLine($"Không tìm thấy sản phẩm có Product ID {updateId} để cập nhật.");
            }
        }

        private void DeleteProduct()
        {
            Console.Write("Nhập Product ID sản phẩm cần xóa: ");
            int deleteId;
            string deleteIdInput = Console.ReadLine();
            if (!ValidationHelper.TryParsePositiveInt(deleteIdInput, out deleteId, "Product ID"))
            {
                return;
            }

            Product productToDelete = _dbHelper.GetProductById(deleteId);
            if (productToDelete == null)
            {
                Console.WriteLine($"Không tìm thấy sản phẩm có Product ID {deleteId}.");
                return;
            }

            // Kiểm tra xem sản phẩm đã được nhập/xuất chưa
            bool hasImported = _dbHelper.HasImportDetails(deleteId);
            bool hasExported = _dbHelper.HasExportDetails(deleteId);

            if (hasImported || hasExported)
            {
                Console.WriteLine("Sản phẩm này đã có chi tiết nhập/xuất kho. Không thể xóa hoàn toàn.");
                // Cập nhật trạng thái thành 0 (không hoạt động)
                _dbHelper.UpdateProductStatus(deleteId, false);
            }
            else
            {
                Console.Write($"Bạn có chắc chắn muốn xóa hoàn toàn sản phẩm '{productToDelete.ProductName}' (ID: {productToDelete.ProductID})? (Y/N): ");
                string confirmation = Console.ReadLine().Trim().ToUpper();
                if (confirmation == "Y")
                {
                    _dbHelper.DeleteProductFromDatabase(deleteId);
                }
                else
                {
                    Console.WriteLine("Hủy bỏ thao tác xóa.");
                }
            }
        }
    }

    // Lớp Program chứa phương thức Main để chạy ứng dụng console
    internal class Program
    {
        static void Main(string[] args)
        {
            // Khởi tạo và chạy ConsoleProductManager
            ConsoleProductManager manager = new ConsoleProductManager();
            manager.Run();
        }
    }
}
