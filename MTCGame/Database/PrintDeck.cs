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
    public class PrintDeck
    {
        public readonly string? Answer;
        public readonly string? Answertemp;
        private static string _Host = "127.0.0.1";
        private static string _User = "postgres";
        public string _DBname = "MTCG";
        private static string _Password = "12345678";
        private static string _Port = "5432";
        public void ToogleTestMode()
        {
            _DBname = "MTCGTests";
        }

        public PrintDeck(string user)
        {

            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine("Opening connection to print deck");
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

                        Answertemp += "{" + card1 + "},{" + card2 + "},{" + card3 + "},{" + card4 + "}";
                    }
                    if (Answertemp == null)
                    { 
                        Answer = "203|The request was fine, but the deck doesn't have any cards|Your deck is empty!";
                    }
                    else
                    { 
                        Answer = "200|The deck has cards, the response contains these|[" + Answertemp + "]";
                    }
                    Console.WriteLine(Answer);
                }
                conn.Close();
            }
        }

        public PrintDeck(string user, bool isTest)
        {
            if(isTest) ToogleTestMode();
            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine("Opening connection to print deck");
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

                        Answertemp += "{" + card1 + "},{" + card2 + "},{" + card3 + "},{" + card4 + "}";
                    }
                    if (Answertemp == null)
                    {
                        Answer = "203|The request was fine, but the deck doesn't have any cards|Your deck is empty!";
                    }
                    else
                    {
                        Answer = "200|The deck has cards, the response contains these|[" + Answertemp + "]";
                    }
                    Console.WriteLine(Answer);
                }
                conn.Close();
            }
        }
    }
}
