namespace CRUDquanao
{
    internal class Program
    {
        static string connectionString = "Server=localhost;Database=managekhoquanao3;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            AddProduct("Apple", 10.5m);
        }

        static void AddProduct(string name, decimal price)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Products (Name, Price) VALUES (@name, @price)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@price", price);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine($"{rows} product(s) added.");
                }
            }
        }
    }
}
