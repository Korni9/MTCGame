using Npgsql;
using System.Numerics;

namespace MTCGame.Database
{
    public class LoginUser
    {
        private string username;
        private string password;
        public readonly string? Answer;
        private static string _Host = "127.0.0.1";
        private static string _User = "postgres";
        public string _DBname = "MTCG";
        private static string _Password = "12345678";
        private static string _Port = "5432";
        public void ToogleTestMode()
        {
            _DBname = "MTCGTests";
        }

        public LoginUser(string username, string password)
        {
            this.username = username;
            this.password = password;
            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine("Opening connection to login user");
                conn.Open();

                Console.WriteLine($"Username: {username}");
                Console.WriteLine($"Passwort: {password}");

                string sql = "SELECT password FROM player WHERE username = @username";
                using (var command = new NpgsqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var passworddb = reader.GetString(0);
                        if (passworddb == password) Answer = "200|User login successful|" + username + "-mtcgToken";
                        else Answer = "401|Invalid username/password provided|Password not correct!";
                    }
                    reader.Close();
                }
                conn.Close();
            }
        }

        public LoginUser(string username, string password, bool isTest)
        {
            if (isTest) ToogleTestMode();
            this.username = username;
            this.password = password;
            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine("Opening connection to login user");
                conn.Open();

                Console.WriteLine($"Username: {username}");
                Console.WriteLine($"Passwort: {password}");

                string sql = "SELECT password FROM player WHERE username = @username";
                using (var command = new NpgsqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var passworddb = reader.GetString(0);
                        if (passworddb == password) Answer = "200|User login successful|" + username + "-mtcgToken";
                        else Answer = "401|Invalid username/password provided|Password not correct!";
                    }
                    reader.Close();
                }
                conn.Close();
            }
        }
    }
}