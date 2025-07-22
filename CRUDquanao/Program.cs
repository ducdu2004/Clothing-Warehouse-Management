using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient; // Thư viện để kết nối với SQL Server
using System.Linq; // Để sử dụng .Any() cho List
using System.Text; // Để sử dụng Encoding.UTF8
using System.Text.RegularExpressions; // Thêm namespace này để sử dụng Regex

namespace CRUDquanao
{
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
