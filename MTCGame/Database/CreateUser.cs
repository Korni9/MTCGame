using Npgsql;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.Arm;

namespace MTCGame.Database
{
    public class CreateUser
    {
        private string Name { get; set; }
        private string Password { get; set; }
        private int Token { get; set; }
        private int Elo { get; set; }
        private int Coins { get; set; }
        private int[] _deck = new int[4];
        private int[] _stack = new int[10];
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

        public CreateUser(string username, string password)
        {
            this.Name = username;
            this.Password = password;
            this.Elo = 100;
            this.Coins = 20;
            string connString =
                $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine("Opening connection to add user");
                conn.Open();

                Console.WriteLine($"Username: {username}");
                Console.WriteLine($"Passwort: {password}");

                string sql = "SELECT COUNT(*) FROM player WHERE username = @username";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    long count = (long)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Answer =
                            "409|User with same username already registered|User already exists in the database.";
                    }
                    else
                    {
                        using var cmd1 = new NpgsqlCommand(
                            "INSERT INTO player (username, password, name, bio, image, elo, wins, losses, coins) VALUES (@username, @password, @name, @bio, @image, @elo, @wins, @losses, @coins)",
                            conn);
                        cmd1.Parameters.AddWithValue("@username", username);
                        cmd1.Parameters.AddWithValue("@password", password);
                        cmd1.Parameters.AddWithValue("@name", "");
                        cmd1.Parameters.AddWithValue("@bio", "");
                        cmd1.Parameters.AddWithValue("@image", "");
                        cmd1.Parameters.AddWithValue("@elo", 100);
                        cmd1.Parameters.AddWithValue("@wins", 0);
                        cmd1.Parameters.AddWithValue("@losses", 0);
                        cmd1.Parameters.AddWithValue("@coins", 20);
                        var workedadduser = cmd1.ExecuteNonQuery();
                        Console.WriteLine($"Number of rows updated={workedadduser}");
                        if (workedadduser == 1)
                        {
                            Answer = "201|User successfully created|User " + username + " has been created!";
                        }
                        else
                        {
                            Answer = "Problem occurred adding user!";
                        }

                        using var cmdcdeck =
                            new NpgsqlCommand(
                                "INSERT INTO deck (card1, card2, card3, card4, userplayer) VALUES (@card1, @card2, @card3, @card4, @userplayer)",
                                conn);
                        cmdcdeck.Parameters.AddWithValue("@card1", "null");
                        cmdcdeck.Parameters.AddWithValue("@card2", "null");
                        cmdcdeck.Parameters.AddWithValue("@card3", "null");
                        cmdcdeck.Parameters.AddWithValue("@card4", "null");
                        cmdcdeck.Parameters.AddWithValue("@userplayer", username);
                        var workedadddeck = cmdcdeck.ExecuteNonQuery();
                        if (workedadddeck == 0)
                        {
                            Answer = "999|Problem occurred creating deck|Problem occurred creating deck!";
                        }
                    }
                }

                conn.Close();
            }
        }

        public CreateUser(string username, string password, bool isTest)
        {
            if(isTest) ToogleTestMode();
            this.Name = username;
            this.Password = password;
            this.Elo = 100;
            this.Coins = 20;
            string connString =
                $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine("Opening connection to add user");
                conn.Open();

                Console.WriteLine($"Username: {username}");
                Console.WriteLine($"Passwort: {password}");

                string sql = "SELECT COUNT(*) FROM player WHERE username = @username";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    long count = (long)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Answer =
                            "409|User with same username already registered|User already exists in the database.";
                    }
                    else
                    {
                        using var cmd1 = new NpgsqlCommand(
                            "INSERT INTO player (username, password, name, bio, image, elo, wins, losses, coins) VALUES (@username, @password, @name, @bio, @image, @elo, @wins, @losses, @coins)",
                            conn);
                        cmd1.Parameters.AddWithValue("@username", username);
                        cmd1.Parameters.AddWithValue("@password", password);
                        cmd1.Parameters.AddWithValue("@name", "");
                        cmd1.Parameters.AddWithValue("@bio", "");
                        cmd1.Parameters.AddWithValue("@image", "");
                        cmd1.Parameters.AddWithValue("@elo", 100);
                        cmd1.Parameters.AddWithValue("@wins", 0);
                        cmd1.Parameters.AddWithValue("@losses", 0);
                        cmd1.Parameters.AddWithValue("@coins", 20);
                        var workedadduser = cmd1.ExecuteNonQuery();
                        Console.WriteLine($"Number of rows updated={workedadduser}");
                        if (workedadduser == 1)
                        {
                            Answer = "201|User successfully created|User " + username + " has been created!";
                        }
                        else
                        {
                            Answer = "Problem occurred adding user!";
                        }

                        using var cmdcdeck =
                            new NpgsqlCommand(
                                "INSERT INTO deck (card1, card2, card3, card4, userplayer) VALUES (@card1, @card2, @card3, @card4, @userplayer)",
                                conn);
                        cmdcdeck.Parameters.AddWithValue("@card1", "null");
                        cmdcdeck.Parameters.AddWithValue("@card2", "null");
                        cmdcdeck.Parameters.AddWithValue("@card3", "null");
                        cmdcdeck.Parameters.AddWithValue("@card4", "null");
                        cmdcdeck.Parameters.AddWithValue("@userplayer", username);
                        var workedadddeck = cmdcdeck.ExecuteNonQuery();
                        if (workedadddeck == 0)
                        {
                            Answer = "999|Problem occurred creating deck|Problem occurred creating deck!";
                        }
                    }
                }

                conn.Close();
            }
        }
    }
}

