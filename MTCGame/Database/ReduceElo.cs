using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGame.Database
{
    internal class ReduceElo
    {
        public readonly string? Answer;
        private static readonly string _Host = "127.0.0.1";
        private static readonly string _User = "postgres";
        private static readonly string _DBname = "MTCG";
        private static readonly string _Password = "12345678";
        private static readonly string _Port = "5432";

        public ReduceElo(string username, string info)
        {
            string jsonString = info;
            JsonElement jsoninfo = JsonSerializer.Deserialize<JsonElement>(jsonString);
            Console.WriteLine(jsoninfo);
            string connString =
                $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine($"Opening connection, update Elo for user: {username}");
                conn.Open();


                using var eloupd = new NpgsqlCommand("UPDATE player SET losses = losses + 1 elo = elo - 3 WHERE username = @username", conn);
                eloupd.Parameters.AddWithValue("@username", username);
                var rowsAffected = eloupd.ExecuteNonQuery();
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
