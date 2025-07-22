using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CRUDquanao
{

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

        // Phương thức kiểm tra xem kích thước có hợp lệ không
        public static bool IsValidSize(string size)
        {
            if (string.IsNullOrWhiteSpace(size)) return false;
            string[] validSizes = { "S", "M", "L", "XL", "XXL" };
            return validSizes.Contains(size.Trim().ToUpper());
        }

        // Phương thức kiểm tra xem màu sắc có hợp lệ không (chỉ chứa chữ cái và không rỗng/khoảng trắng)
        public static bool IsValidColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color)) return false;
            // Kiểm tra xem chuỗi chỉ chứa các ký tự chữ cái (bao gồm cả tiếng Việt nếu font hỗ trợ)
            return Regex.IsMatch(color, @"^[a-zA-ZÀÁẠẢÃĂẰẮẶẲẴÂẦẤẬẨẪÈÉẸẺẼÊỀẾỆỂỄÌÍỊỈĨÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠÙÚỤỦŨƯỪỨỰỬỮỲÝỴỶỸĐđ\s]+$");
        }
    }
}
