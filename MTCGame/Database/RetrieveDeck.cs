using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTCGame.Database
{
    internal class RetrieveDeck
    {
        public readonly string? Deckjson = "[";
        public readonly string? Temp;
        private List<string> cardList = new();
        private static readonly string _Host = "127.0.0.1";
        private static readonly string _User = "postgres";
        private static readonly string _DBname = "MTCG";
        private static readonly string _Password = "12345678";
        private static readonly string _Port = "5432";

        public RetrieveDeck(string user)
        {
            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine("Opening connection to retrieve deck");
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT card1, card2, card3, card4 FROM deck WHERE userplayer = @userplayer", conn))
                {
                    cmd.Parameters.AddWithValue("@userplayer", user);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string? card1 = reader.GetString(0);
                        string? card2 = reader.GetString(1);
                        string? card3 = reader.GetString(2);
                        string? card4 = reader.GetString(3);

                        Temp = "{" + card1 + "},{" + card2 + "},{" + card3 + "},{" + card4 + "}";
                        cardList.Add(card1);
                        cardList.Add(card2);
                        cardList.Add(card3);
                        cardList.Add(card4);
                    }
                    reader.Close();
                }
                Console.WriteLine("Opening connection to retrieve card information");

                foreach (var card in cardList)
                {
                    using (var cmdcard = new NpgsqlCommand("SELECT name, damage FROM card WHERE id = @id", conn))
                    {
                        string trim = card.Trim('"');
                        cmdcard.Parameters.AddWithValue("@id", trim);
                        var reader = cmdcard.ExecuteReader();
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);
                            double damage = reader.GetDouble(1);

                            Deckjson += "{\"Id\":" + card + ",\"Name\":\"" + name +"\",\"Damage\":" + damage + "},";
                        }
                        reader.Close();
                    }
                }
                Deckjson = Deckjson.Remove(Deckjson.Length - 1);
                Deckjson += "]";
                conn.Close();
            }
        }
    }
}
