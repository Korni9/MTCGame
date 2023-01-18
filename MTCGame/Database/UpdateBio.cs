using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGame.Database
{
    public class UpdateBio
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

        public UpdateBio(string username, string info)
        {
            string jsonString = info;
            JsonElement jsoninfo = JsonSerializer.Deserialize<JsonElement>(jsonString);
            Console.WriteLine(jsoninfo);
            string connString =
                $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine($"Opening connection, read Bio for user: {username}");
                conn.Open();

                using var bioupd = new NpgsqlCommand("UPDATE player SET name = @name, bio = @bio, image = @image WHERE username = @username", conn);
                bioupd.Parameters.AddWithValue("@name", jsoninfo.GetProperty("Name").GetString());
                bioupd.Parameters.AddWithValue("@bio", jsoninfo.GetProperty("Bio").GetString());
                bioupd.Parameters.AddWithValue("@image", jsoninfo.GetProperty("Image").GetString());
                bioupd.Parameters.AddWithValue("@username", username);
                bioupd.Prepare();
                var rowsAffected = bioupd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Answer = "200|User sucessfully updated.|User Information have been updated.";
                }
                else
                {
                    Answer = "404|User not found.|Error adding bio information!";
                }
                conn.Close();
            }
        }

        public UpdateBio(string username, string info, bool isTest)
        {
            if(isTest) ToogleTestMode();

            string jsonString = info;
            JsonElement jsoninfo = JsonSerializer.Deserialize<JsonElement>(jsonString);
            Console.WriteLine(jsoninfo);
            string connString =
                $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine($"Opening connection, read Bio for user: {username}");
                conn.Open();

                using var bioupd = new NpgsqlCommand("UPDATE player SET name = @name, bio = @bio, image = @image WHERE username = @username", conn);
                bioupd.Parameters.AddWithValue("@name", jsoninfo.GetProperty("Name").GetString());
                bioupd.Parameters.AddWithValue("@bio", jsoninfo.GetProperty("Bio").GetString());
                bioupd.Parameters.AddWithValue("@image", jsoninfo.GetProperty("Image").GetString());
                bioupd.Parameters.AddWithValue("@username", username);
                bioupd.Prepare();
                var rowsAffected = bioupd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Answer = "200|User sucessfully updated.|User Information have been updated.";
                }
                else
                {
                    Answer = "404|User not found.|Error adding bio information!";
                }
                conn.Close();
            }
        }
    }
}
