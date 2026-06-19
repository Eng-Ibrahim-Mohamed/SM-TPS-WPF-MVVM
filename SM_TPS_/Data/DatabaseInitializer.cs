using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SM_TPS_.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                @"CREATE TABLE IF NOT EXISTS Products
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Barcode TEXT,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL,
                    StockQuantity INTEGER NOT NULL
                );";
                string transactionTable =
                @"CREATE TABLE IF NOT EXISTS Transactions
                (
                      Id INTEGER PRIMARY KEY AUTOINCREMENT,
                      TransactionDate TEXT NOT NULL,
                       Total REAL NOT NULL
                );";
                string usersTable =
                 @"CREATE TABLE IF NOT EXISTS Users
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    Password TEXT NOT NULL,
                    Role TEXT NOT NULL
                );";

                SQLiteCommand usersCmd =
                    new SQLiteCommand(usersTable, connection);

                usersCmd.ExecuteNonQuery();

                string insertAdmin =
                    @"INSERT OR IGNORE INTO Users
                    (
                        Id, Username, Password, Role)
                    VALUES
                    (1, 'admin', '1234', 'Admin');";
                SQLiteCommand adminCmd =
                    new SQLiteCommand(insertAdmin, connection);

                adminCmd.ExecuteNonQuery();

                usersCmd.ExecuteNonQuery();
                SQLiteCommand transactionCmd =
                    new SQLiteCommand(transactionTable, connection);

                transactionCmd.ExecuteNonQuery();

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Database Initialized");
            }
        }
    }
}