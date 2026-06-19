using SM_TPS_.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_TPS_.Data
{
    public class UserRepository
    {
        public bool Login(string username,
                          string password)
        {
            using (var connection =
                DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                @"SELECT COUNT(*)
                  FROM Users
                  WHERE Username=@Username
                  AND Password=@Password";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue(
                    "@Username", username);

                cmd.Parameters.AddWithValue(
                    "@Password", password);

                int count =
                    Convert.ToInt32(
                        cmd.ExecuteScalar());

                return count > 0;
            }
        }
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (var connection =
                DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                    "SELECT * FROM Users";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                SQLiteDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString()
                    });
                }
            }

            return users;
        }
        public void AddUser(User user)
        {
            using (var connection =
                DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                @"INSERT INTO Users
                (Username, Password, Role)
                VALUES
                (@Username, @Password, @Role)";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue(
                    "@Username", user.Username);

                cmd.Parameters.AddWithValue(
                    "@Password", user.Password);

                cmd.Parameters.AddWithValue(
                    "@Role", user.Role);

                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteUser(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                "DELETE FROM Users WHERE Id = @Id";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }
        }
        public void ResetPassword(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                "UPDATE Users SET Password='1234' WHERE Id=@Id";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }
        }
        public void ChangePassword(int id, string password)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                @"UPDATE Users
          SET Password = @Password
          WHERE Id = @Id";

                SQLiteCommand cmd =
                    new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue(
                    "@Password", password);

                cmd.Parameters.AddWithValue(
                    "@Id", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}