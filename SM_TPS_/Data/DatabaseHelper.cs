using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace SM_TPS_.Data
{
    public static class DatabaseHelper
    {
        private static string connectionString =
            "Data Source=store.db;Version=3;";

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}