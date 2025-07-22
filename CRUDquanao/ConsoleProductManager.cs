using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDquanao
{
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
            string productName;
            do
            {
                productName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(productName))
                {
                    Console.WriteLine("Tên sản phẩm không được để trống. Vui lòng nhập tên sản phẩm:");
                }
            } while (string.IsNullOrWhiteSpace(productName));


            int categoryID;
            string categoryIDInput;
            do
            {
                Console.Write("Nhập Category ID: ");
                categoryIDInput = Console.ReadLine();
                if (!ValidationHelper.TryParsePositiveInt(categoryIDInput, out categoryID, "Category ID"))
                {
                    // Lỗi định dạng, yêu cầu nhập lại
                    categoryID = 0; // Đặt về 0 để vòng lặp tiếp tục
                    continue;
                }

                // Kiểm tra sự tồn tại của CategoryID
                if (!_dbHelper.CategoryExists(categoryID))
                {
                    Console.WriteLine($"Lỗi: Category ID '{categoryID}' không tồn tại.");
                    Console.WriteLine("Các Category hiện có:");
                    List<ProductCategory> categories = _dbHelper.GetAllCategories();
                    if (categories.Any())
                    {
                        foreach (var cat in categories)
                        {
                            Console.WriteLine($"- ID: {cat.CategoryID}, Tên: {cat.CategoryName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Chưa có Category nào trong cơ sở dữ liệu.");
                    }
                    Console.WriteLine("Vui lòng nhập một Category ID hợp lệ:");
                    categoryID = 0; // Đặt về 0 để vòng lặp tiếp tục
                }
            } while (categoryID == 0); // Lặp lại nếu CategoryID không hợp lệ hoặc không tồn tại


            string size;
            do
            {
                Console.Write("Nhập kích thước (Size - S, M, L, XL, XXL): ");
                size = Console.ReadLine();
                if (!ValidationHelper.IsValidSize(size))
                {
                    Console.WriteLine("Kích thước không hợp lệ. Vui lòng nhập S, M, L, XL, hoặc XXL.");
                }
            } while (!ValidationHelper.IsValidSize(size));


            string color;
            do
            {
                Console.Write("Nhập màu sắc (Color): ");
                color = Console.ReadLine();
                if (!ValidationHelper.IsValidColor(color))
                {
                    Console.WriteLine("Màu sắc không hợp lệ. Vui lòng chỉ nhập chữ cái (ví dụ: Đỏ, Xanh).");
                }
            } while (!ValidationHelper.IsValidColor(color));


            decimal price;
            string priceInput;
            do
            {
                Console.Write("Nhập giá sản phẩm: ");
                priceInput = Console.ReadLine();
            } while (!ValidationHelper.TryParsePositiveDecimal(priceInput, out price, "Giá sản phẩm"));


            int quantity;
            string quantityInput;
            do
            {
                Console.Write("Nhập số lượng (Quantity): ");
                quantityInput = Console.ReadLine();
            } while (!ValidationHelper.TryParseNonNegativeInt(quantityInput, out quantity, "Số lượng"));


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
                    // Lặp cho đến khi Category ID hợp lệ hoặc người dùng để trống
                    bool categoryIdValid = false;
                    do
                    {
                        if (ValidationHelper.TryParsePositiveInt(newCategoryIDInput, out newCategoryID, "Category ID mới"))
                        {
                            if (_dbHelper.CategoryExists(newCategoryID))
                            {
                                productToUpdate.CategoryID = newCategoryID;
                                categoryIdValid = true;
                            }
                            else
                            {
                                Console.WriteLine($"Lỗi: Category ID '{newCategoryID}' không tồn tại.");
                                Console.WriteLine("Các Category hiện có:");
                                List<ProductCategory> categories = _dbHelper.GetAllCategories();
                                if (categories.Any())
                                {
                                    foreach (var cat in categories)
                                    {
                                        Console.WriteLine($"- ID: {cat.CategoryID}, Tên: {cat.CategoryName}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Chưa có Category nào trong cơ sở dữ liệu.");
                                }
                                Console.WriteLine("Vui lòng nhập một Category ID hợp lệ hoặc để trống:");
                                newCategoryIDInput = Console.ReadLine();
                                if (string.IsNullOrWhiteSpace(newCategoryIDInput)) // Người dùng chọn không đổi
                                {
                                    categoryIdValid = true;
                                }
                            }
                        }
                        else
                        {
                            Console.Write("Nhập Category ID mới (để trống nếu không đổi): ");
                            newCategoryIDInput = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newCategoryIDInput)) // Người dùng chọn không đổi
                            {
                                categoryIdValid = true;
                            }
                        }
                    } while (!categoryIdValid);
                }

                Console.WriteLine($"Kích thước hiện tại: {productToUpdate.Size}");
                string newSize;
                do
                {
                    Console.Write("Nhập kích thước mới (S, M, L, XL, XXL, để trống nếu không đổi): ");
                    newSize = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newSize)) // Người dùng chọn không đổi
                    {
                        break;
                    }
                    if (!ValidationHelper.IsValidSize(newSize))
                    {
                        Console.WriteLine("Kích thước không hợp lệ. Vui lòng nhập S, M, L, XL, XXL, hoặc để trống.");
                    }
                } while (!ValidationHelper.IsValidSize(newSize));
                if (!string.IsNullOrWhiteSpace(newSize))
                {
                    productToUpdate.Size = newSize;
                }


                Console.WriteLine($"Màu sắc hiện tại: {productToUpdate.Color}");
                string newColor;
                do
                {
                    Console.Write("Nhập màu sắc mới (để trống nếu không đổi): ");
                    newColor = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newColor)) // Người dùng chọn không đổi
                    {
                        break;
                    }
                    if (!ValidationHelper.IsValidColor(newColor))
                    {
                        Console.WriteLine("Màu sắc không hợp lệ. Vui lòng chỉ nhập chữ cái (ví dụ: Đỏ, Xanh) hoặc để trống.");
                    }
                } while (!ValidationHelper.IsValidColor(newColor));
                if (!string.IsNullOrWhiteSpace(newColor))
                {
                    productToUpdate.Color = newColor;
                }


                Console.WriteLine($"Giá hiện tại: {productToUpdate.Price}");
                string newPriceInput;
                decimal newPrice;
                bool priceValid = false;
                do
                {
                    Console.Write("Nhập giá mới (để trống nếu không đổi): ");
                    newPriceInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newPriceInput))
                    {
                        priceValid = true; // Người dùng chọn không đổi
                    }
                    else if (ValidationHelper.TryParsePositiveDecimal(newPriceInput, out newPrice, "Giá mới"))
                    {
                        productToUpdate.Price = newPrice;
                        priceValid = true;
                    }
                } while (!priceValid);


                Console.WriteLine($"Số lượng hiện tại: {productToUpdate.Quantity}");
                string newQuantityInput;
                int newQuantity;
                bool quantityValid = false;
                do
                {
                    Console.Write("Nhập số lượng mới (để trống nếu không đổi): ");
                    newQuantityInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newQuantityInput))
                    {
                        quantityValid = true; // Người dùng chọn không đổi
                    }
                    else if (ValidationHelper.TryParseNonNegativeInt(newQuantityInput, out newQuantity, "Số lượng mới"))
                    {
                        productToUpdate.Quantity = newQuantity;
                        quantityValid = true;
                    }
                } while (!quantityValid);


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
                    string newStatusInput;
                    bool newStatus;
                    bool statusValid = false;
                    do
                    {
                        Console.Write("Cập nhật trạng thái (true/false, để trống nếu không đổi): ");
                        newStatusInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newStatusInput))
                        {
                            statusValid = true; // Người dùng chọn không đổi
                        }
                        else if (bool.TryParse(newStatusInput, out newStatus))
                        {
                            productToUpdate.Status = newStatus;
                            statusValid = true;
                        }
                        else
                        {
                            Console.WriteLine("Trạng thái không hợp lệ. Vui lòng nhập 'true', 'false' hoặc để trống.");
                        }
                    } while (!statusValid);
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
}
