using Microsoft.VisualBasic;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Database
{
    internal class BuyPackage
    {
        public readonly string? Answer;
        private int[] _deck = new int[4];
        private int[] _stack = new int[10];
        private static readonly string _Host = "127.0.0.1";
        private static readonly string _User = "postgres";
        private static readonly string _DBname = "MTCG";
        private static readonly string _Password = "12345678";
        private static readonly string _Port = "5432";

        public BuyPackage(string user)
        {
            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";
            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine("Opening connection to buy package");
                conn.Open();
                int firstId;
                using var cmdcoinret = new NpgsqlCommand("SELECT coins FROM player WHERE username = @username", conn);
                cmdcoinret.Parameters.AddWithValue("@username", user);
                int coins = (int)cmdcoinret.ExecuteScalar();
                if (coins < 5)
                {
                    Answer =
                        "403|Not enough money for buying a card package|You do not have enough coins. Pack costs 5 coins.";
                }
                else
                {
                    using var cmdcheck = new NpgsqlCommand("SELECT COUNT(*) FROM packs", conn);
                    long count = (long)cmdcheck.ExecuteScalar();
                    if (count == 0)
                    {
                        Answer = "404|No card package available for buying|No packages exist, please contact administrator";
                    }
                    else
                    {
                        using var cmdcoins =
                            new NpgsqlCommand("UPDATE player SET coins = coins - 5 WHERE username = @user;", conn);
                        cmdcoins.Parameters.AddWithValue("@user", user);
                        cmdcoins.ExecuteNonQuery();

                        using var cmdretr =
                            new NpgsqlCommand(
                                "SELECT MIN(id) FROM packs WHERE id NOT IN (SELECT id FROM packs GROUP BY id HAVING COUNT(id) > 1)",
                                conn);
                        firstId = (int)cmdretr.ExecuteScalar();
                        Console.WriteLine("First used ID: " + firstId);

                        Console.WriteLine(user);
                        Console.WriteLine("pack" + firstId);
                        using var cmdupd = new NpgsqlCommand("UPDATE card SET owner = @user WHERE owner = @oldowner",
                            conn);

                        cmdupd.Parameters.AddWithValue("@user", user);
                        cmdupd.Parameters.AddWithValue("@oldowner", "pack" + firstId);
                        var workedupd = cmdupd.ExecuteNonQuery();
                        Console.WriteLine($"Number of rows updated={workedupd}");
                        if (workedupd > 0)
                        {
                            Answer = "200|A package has been successfully bought|Package bought!";
                        }
                        else
                        {
                            Answer = "999|Problem occurred buying package|Problem occurred buying package!";
                        }

                        using var cmddel = new NpgsqlCommand("DELETE FROM packs WHERE id = @id", conn);
                        cmddel.Parameters.AddWithValue("@id", firstId);
                        cmddel.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }
    }
}
