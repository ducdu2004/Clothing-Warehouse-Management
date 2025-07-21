using System.Text.RegularExpressions;
using quanliquanao3.Models;

namespace CourseManagemanet
{
    public static class ValidationHelper
    {
        private static readonly Regex NameRegex = new Regex(@"^[\p{L}\s\.'-]*$", RegexOptions.Compiled);
        public static bool IsPositiveInteger(string valueString, string fieldName, out int result, out string errorMessage, int min = 1, int max = int.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(valueString))
            {
                result = 0;
                errorMessage = $"{fieldName} không được để trống.";
                return false;
            }

            if (!int.TryParse(valueString, out result))
            {
                errorMessage = $"{fieldName} phải là một số nguyên hợp lệ.";
                return false;
            }

            if (result < min || result > max)
            {
                errorMessage = $"{fieldName} phải nằm trong khoảng từ {min} đến {max}.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
        public static bool IsPositiveDecimal(string valueString, string fieldName, out decimal result, out string errorMessage, decimal min = 0, decimal max = decimal.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(valueString))
            {
                result = 0;
                errorMessage = $"{fieldName} không được để trống.";
                return false;
            }
            if (!decimal.TryParse(valueString, out result))
            {
                errorMessage = $"{fieldName} phải là một số thập phân hợp lệ.";
                return false;
            }
            if (result < min || result > max)
            {
                errorMessage = $"{fieldName} phải nằm trong khoảng từ {min} đến {max}.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }


        public static bool IsStudentIdUnique(int ProductId, Managekhoquanao3Context context , out string errorMessage)
        {
            if (context.Products.Any(s => s.ProductId == ProductId))
            {
                errorMessage = $"Mã sinh viên '{ProductId}' đã tồn tại. Vui lòng nhập mã khác.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }



        public static bool IsStringValid(string value, string fieldName, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                errorMessage = $"{fieldName} không được để trống.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool IsValidPersonName(string value, string fieldName, out string errorMessage, bool isRequired = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (isRequired)
                {
                    errorMessage = $"{fieldName} không được để trống.";
                    return false;
                }
                errorMessage = string.Empty; // Nếu không bắt buộc và trống thì vẫn hợp lệ
                return true;
            }

            if (value.Length > 50) // Ví dụ: giới hạn độ dài tên
            {
                errorMessage = $"{fieldName} không được vượt quá 50 ký tự.";
                return false;
            }

            if (!NameRegex.IsMatch(value))
            {
                errorMessage = $"{fieldName} chỉ được chứa chữ cái, khoảng trắng, dấu chấm, dấu nháy đơn hoặc dấu gạch nối.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }



    }
}