using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGame.Database
{
    public class ConfigureDeck
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

        public ConfigureDeck(string user, string body)
        {
            string connString =
                $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine("Opening connection to configure deck");
                conn.Open();
                string[] separatoreck = { "[", "]" };
                var packagesparsed = body.Split(separatoreck, StringSplitOptions.TrimEntries);
                var cardsdecklist = packagesparsed[1].Split(", ");
                if (cardsdecklist.Length != 4)
                {
                    Answer =
                        "400|The provided deck did not include the required amount of cards|The provided package does not contain 4 cards!";
                }
                else
                {
                    using var cmddP =
                        new NpgsqlCommand(
                            "UPDATE deck SET card1 = @card1, card2 = @card2, card3 = @card3, card4 = @card4 WHERE userplayer = @userplayer",
                            conn);
                    cmddP.Parameters.AddWithValue("@card1", cardsdecklist[0]);
                    cmddP.Parameters.AddWithValue("@card2", cardsdecklist[1]);
                    cmddP.Parameters.AddWithValue("@card3", cardsdecklist[2]);
                    cmddP.Parameters.AddWithValue("@card4", cardsdecklist[3]);
                    cmddP.Parameters.AddWithValue("@userplayer", user);
                    cmddP.ExecuteNonQuery();

                    var worked = cmddP.ExecuteNonQuery();
                    Console.WriteLine($"Number of rows updated={worked}");
                    if (worked == 1)
                    {
                        Answer = "201|Package and cards successfully created|Package added!";
                    }
                    else
                    {
                        Answer = "999|Problem occurred adding package|Problem occurred adding package!";
                    }
                }
            }
        }

        public ConfigureDeck(string user, string body, bool isTest)
        {
            if(isTest) ToogleTestMode();
            string connString =
                $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine("Opening connection to configure deck");
                conn.Open();
                string[] separatoreck = { "[", "]" };
                var packagesparsed = body.Split(separatoreck, StringSplitOptions.TrimEntries);
                var cardsdecklist = packagesparsed[1].Split(", ");
                if (cardsdecklist.Length != 4)
                {
                    Answer =
                        "400|The provided deck did not include the required amount of cards|The provided package does not contain 4 cards!";
                }
                else
                {
                    using var cmddP =
                        new NpgsqlCommand(
                            "UPDATE deck SET card1 = @card1, card2 = @card2, card3 = @card3, card4 = @card4 WHERE userplayer = @userplayer",
                            conn);
                    cmddP.Parameters.AddWithValue("@card1", cardsdecklist[0]);
                    cmddP.Parameters.AddWithValue("@card2", cardsdecklist[1]);
                    cmddP.Parameters.AddWithValue("@card3", cardsdecklist[2]);
                    cmddP.Parameters.AddWithValue("@card4", cardsdecklist[3]);
                    cmddP.Parameters.AddWithValue("@userplayer", user);
                    cmddP.ExecuteNonQuery();

                    var worked = cmddP.ExecuteNonQuery();
                    Console.WriteLine($"Number of rows updated={worked}");
                    if (worked == 1)
                    {
                        Answer = "201|Package and cards successfully created|Package added!";
                    }
                    else
                    {
                        Answer = "999|Problem occurred adding package|Problem occurred adding package!";
                    }
                }
            }
        }
    }
}
