using System;
using System.Collections.Generic;
using System.Data.SqlClient; // Sử dụng Microsoft.Data.SqlClient
using System.Linq; // Để sử dụng .Any() cho List
using Microsoft.Data.SqlClient; // Thư viện để kết nối với SQL Server
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
                    connection.Open(); // Mở kết nối đến cơ sở dữ liệu

                    // Câu lệnh SQL để chèn dữ liệu vào bảng Products với tất cả các cột
                    string query = "INSERT INTO Products (ProductName, CategoryID, Size, Color, Price, Quantity) VALUES (@ProductName, @CategoryID, @Size, @Color, @Price, @Quantity)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm các tham số để tránh SQL Injection
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                        command.Parameters.AddWithValue("@Size", product.Size);
                        command.Parameters.AddWithValue("@Color", product.Color);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Quantity", product.Quantity);

                        int rowsAffected = command.ExecuteNonQuery(); // Thực thi câu lệnh
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
                    // Câu lệnh SQL để chọn tất cả sản phẩm với các trường
                    string query = "SELECT ProductID, ProductName, CategoryID, Size, Color, Price, Quantity FROM Products";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader()) // Thực thi và đọc dữ liệu
                        {
                            while (reader.Read()) // Đọc từng hàng
                            {
                                products.Add(new Product
                                {
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                    Size = reader.GetString(reader.GetOrdinal("Size")),
                                    Color = reader.GetString(reader.GetOrdinal("Color")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
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
    }

    // Lớp Program chứa phương thức Main để chạy ứng dụng console
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            bool running = true;

            Console.OutputEncoding = System.Text.Encoding.UTF8; // Đảm bảo hiển thị tiếng Việt đúng cách

            while (running)
            {
                Console.WriteLine("\n--- ỨNG DỤNG QUẢN LÝ SẢN PHẨM ---");
                Console.WriteLine("1. Xem danh sách sản phẩm");
                Console.WriteLine("2. Thêm sản phẩm mới");
                Console.WriteLine("3. Thoát");
                Console.Write("Chọn một tùy chọn: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        List<Product> products = dbHelper.GetAllProducts();
                        if (products.Any())
                        {
                            Console.WriteLine("\n--- DANH SÁCH SẢN PHẨM ---");
                            Console.WriteLine("{0,-5} {1,-25} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10}", "ID", "Tên SP", "Cat ID", "Size", "Color", "Giá", "SL");
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            foreach (var product in products)
                            {
                                Console.WriteLine("{0,-5} {1,-25} {2,-10} {3,-10} {4,-10} {5,-10:N0} {6,-10}",
                                    product.ProductID, product.ProductName, product.CategoryID, product.Size, product.Color, product.Price, product.Quantity);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không có sản phẩm nào trong cơ sở dữ liệu.");
                        }
                        break;
                    case "2":
                        Console.Write("Nhập tên sản phẩm: ");
                        string productName = Console.ReadLine();
                        Console.Write("Nhập Category ID: ");
                        int categoryID;
                        while (!int.TryParse(Console.ReadLine(), out categoryID) || categoryID <= 0)
                        {
                            Console.WriteLine("Category ID không hợp lệ. Vui lòng nhập một số nguyên dương.");
                            Console.Write("Nhập Category ID: ");
                        }
                        Console.Write("Nhập kích thước (Size): ");
                        string size = Console.ReadLine();
                        Console.Write("Nhập màu sắc (Color): ");
                        string color = Console.ReadLine();
                        Console.Write("Nhập giá sản phẩm: ");
                        decimal price;
                        while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
                        {
                            Console.WriteLine("Giá không hợp lệ. Vui lòng nhập một số dương.");
                            Console.Write("Nhập giá sản phẩm: ");
                        }
                        Console.Write("Nhập số lượng (Quantity): ");
                        int quantity;
                        while (!int.TryParse(Console.ReadLine(), out quantity) || quantity < 0)
                        {
                            Console.WriteLine("Số lượng không hợp lệ. Vui lòng nhập một số nguyên không âm.");
                            Console.Write("Nhập số lượng (Quantity): ");
                        }

                        dbHelper.AddProduct(new Product
                        {
                            ProductName = productName,
                            CategoryID = categoryID,
                            Size = size,
                            Color = color,
                            Price = price,
                            Quantity = quantity
                        });
                        break;
                    case "3":
                        running = false;
                        Console.WriteLine("Đang thoát ứng dụng. Tạm biệt!");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                        break;
                }
            }
        }
    }
}
