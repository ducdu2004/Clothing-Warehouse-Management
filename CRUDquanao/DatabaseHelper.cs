using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace CRUDquanao
{
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

        // Phương thức lấy tất cả Category (ID và Name)
        public List<ProductCategory> GetAllCategories()
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT CategoryID, CategoryName FROM ProductCategories";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(new ProductCategory
                                {
                                    CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi khi lấy danh sách Category: {ex.Message}");
                    }
                }
            }
            return categories;
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
}
