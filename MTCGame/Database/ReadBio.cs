using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Npgsql;

namespace MTCGame.Database
{
    public class ReadBio
    {
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
        public ReadBio(string username)
        {
            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine($"Opening connection, read Bio for user: {username}");
                conn.Open();
                string sql = $"SELECT name, bio, image FROM player WHERE username = @player";
                using (var command = new NpgsqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@player", username);
                    Answer = "200|Data successfully retrieved|[";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string bio = reader.GetString(1);
                        string image = reader.GetString(2);
                        JsonObject json = new JsonObject
                        {
                            { "name", JsonValue.Create(name) },
                            { "bio", JsonValue.Create(bio) },
                            { "image", JsonValue.Create(image) }
                        };
                        string jsonString = json.ToString();
                        Answer += jsonString + ",";
                    }
                    Answer = Answer.Remove(Answer.Length - 1);
                    Answer += "]";
                    reader.Close();
                }
                conn.Close();
            }
        }
        public ReadBio(string username, bool isTest)
        {
            if (isTest) ToogleTestMode();

            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine($"Opening connection, read Bio for user: {username}");
                conn.Open();
                string sql = $"SELECT name, bio, image FROM player WHERE username = @player";
                using (var command = new NpgsqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@player", username);
                    Answer = "200|Data successfully retrieved|[";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string bio = reader.GetString(1);
                        string image = reader.GetString(2);
                        JsonObject json = new JsonObject
                        {
                            { "name", JsonValue.Create(name) },
                            { "bio", JsonValue.Create(bio) },
                            { "image", JsonValue.Create(image) }
                        };
                        string jsonString = json.ToString();
                        Answer += jsonString + ",";
                    }
                    Answer = Answer.Remove(Answer.Length - 1);
                    Answer += "]";
                    reader.Close();
                }
                conn.Close();
            }
        }
    }
}
 
