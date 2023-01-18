using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MTCGame.Database
{
    internal class PrintAllPlayerCards
    {
        public readonly string? Answer;
        private static readonly string _Host = "127.0.0.1";
        private static readonly string _User = "postgres";
        private static readonly string _DBname = "MTCG";
        private static readonly string _Password = "12345678";
        private static readonly string _Port = "5432";

        public PrintAllPlayerCards(string user)
        {
            string connString =
                $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using var cmdcards = new NpgsqlCommand("SELECT name, damage FROM card WHERE owner = @user", conn);
                cmdcards.Parameters.AddWithValue("@user", user);
                Answer = "200|The user has cards, the response contains these|[";
                var reader = cmdcards.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    double damage = reader.GetDouble(1);

                    JsonObject json = new JsonObject
                    {
                        { "name", JsonValue.Create(name)},
                        { "damage", JsonValue.Create(damage)}
                    };

                    string card = json.ToString();

                    Answer += card+",";
                }
                Answer = Answer.Remove(Answer.Length - 1);
                Answer += "]";
                reader.Close();
            }
        }
    }
}
