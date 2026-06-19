using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using SM_TPS_.Models;

namespace SM_TPS_.Data
{
    public class TransactionRepository
    {
        public void AddTransaction(decimal total)
        {
            using (var connection =
                DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                @"INSERT INTO Transactions
                (TransactionDate, Total)
                VALUES
                (@Date, @Total)";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue(
                    "@Date",
                    System.DateTime.Now.ToString());

                cmd.Parameters.AddWithValue(
                    "@Total",
                    total);

                cmd.ExecuteNonQuery();
            }
        }
        public List<TransactionRecord> GetAllTransactions()
        {
            List<TransactionRecord> transactions =
                new List<TransactionRecord>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                "SELECT * FROM Transactions ORDER BY Id DESC";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                SQLiteDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    transactions.Add(new TransactionRecord
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        TransactionDate =
                            reader["TransactionDate"].ToString(),
                        Total =
                            Convert.ToDecimal(reader["Total"])
                    });
                }
            }

            return transactions;
        }
    }
}