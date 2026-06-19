using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using SM_TPS_.Models;

namespace SM_TPS_.Data
{
    public class ProductRepository
    {
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Products";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                SQLiteDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Barcode = reader["Barcode"].ToString(),
                            Name = reader["Name"].ToString(),
                            Price = decimal.Parse(reader["Price"].ToString()),
                            StockQuantity = int.Parse(reader["StockQuantity"].ToString())
                        });
                    }
                }
            }

            return products;
        }
        public void AddProduct(Product product)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                             @"INSERT INTO Products
                              (Barcode, Name, Price, StockQuantity)
                              VALUES
                              (@Barcode, @Name, @Price, @StockQuantity)";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                cmd.Parameters.AddWithValue("@Barcode", product.Barcode);

                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteProduct(string productName)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                "DELETE FROM Products WHERE Name = @Name";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@Name", productName);

                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateProduct(Product product)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                @"UPDATE Products
                 SET Name = @Name,
                 Price = @Price,
                StockQuantity = @StockQuantity
                WHERE Id = @Id";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                cmd.Parameters.AddWithValue("@Id", product.Id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}