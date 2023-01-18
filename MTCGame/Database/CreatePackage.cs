using MTCGame.Server;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGame.Database
{
    public class CreatePackage
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

        public CreatePackage(string packagep)
        {
            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                string[] separatorCards = { "{", "}, ", "}" };
                string[] cardscreatelist = packagep.Split(separatorCards, StringSplitOptions.RemoveEmptyEntries);
                foreach (var card in cardscreatelist)
                {
                    //Console.WriteLine(card);
                    var checkexists = new checkCardifExists(card, conn);
                    if (checkexists.ifExists)
                    {
                        Answer =
                            "409|At least one card in the packages already exists|Corrupt Package, at least one Card is already in the database!";
                        return;
                    }
                }
                int lastId;
                if (Answer == null)
                {

                    using var cmdiPack = new NpgsqlCommand("INSERT INTO packs (packname) VALUES (@packname)", conn);
                    cmdiPack.Parameters.AddWithValue("@packname", "pack");
                    cmdiPack.ExecuteNonQuery();

                    using var cmdretr = new NpgsqlCommand("SELECT id FROM packs ORDER BY id DESC LIMIT 1", conn);
                    lastId = (int)cmdretr.ExecuteScalar();
                    Console.WriteLine("Last used ID: " + lastId);

                    using var cmdid = new NpgsqlCommand("UPDATE packs SET packname = @packname WHERE id = @id", conn);
                    cmdid.Parameters.AddWithValue("@packname", "pack" + lastId);
                    cmdid.Parameters.AddWithValue("@id", lastId);
                    cmdid.ExecuteNonQuery();

                    foreach (var card in cardscreatelist)
                    {
                        string cardstring = "{" + card + "}";
                        Cards cardjson = JsonSerializer.Deserialize<Cards>(cardstring);
                        using var cmdiP = new NpgsqlCommand("INSERT INTO card (id, name, damage, owner) VALUES (@id, @name, @damage, @owner)", conn);
                        cmdiP.Parameters.AddWithValue("@id", cardjson.Id);
                        cmdiP.Parameters.AddWithValue("@name", cardjson.Name);
                        cmdiP.Parameters.AddWithValue("@damage", cardjson.Damage);
                        cmdiP.Parameters.AddWithValue("@owner", "pack" + lastId);

                        var worked = cmdiP.ExecuteNonQuery();
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
                conn.Close();
            }
        }
        public CreatePackage(string packagep, bool isTest)
        {
            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                if (isTest) ToogleTestMode();
                conn.Open();

                string[] separatorCards = { "{", "}, ", "}" };
                string[] cardscreatelist = packagep.Split(separatorCards, StringSplitOptions.RemoveEmptyEntries);
                foreach (var card in cardscreatelist)
                {
                    //Console.WriteLine(card);
                    var checkexists = new checkCardifExists(card, conn);
                    if (checkexists.ifExists)
                    {
                        Answer =
                            "409|At least one card in the packages already exists|Corrupt Package, at least one Card is already in the database!";
                        return;
                    }
                }
                int lastId;
                if (Answer == null)
                {

                    using var cmdiPack = new NpgsqlCommand("INSERT INTO packs (packname) VALUES (@packname)", conn);
                    cmdiPack.Parameters.AddWithValue("@packname", "pack");
                    cmdiPack.ExecuteNonQuery();

                    using var cmdretr = new NpgsqlCommand("SELECT id FROM packs ORDER BY id DESC LIMIT 1", conn);
                    lastId = (int)cmdretr.ExecuteScalar();
                    Console.WriteLine("Last used ID: " + lastId);

                    using var cmdid = new NpgsqlCommand("UPDATE packs SET packname = @packname WHERE id = @id", conn);
                    cmdid.Parameters.AddWithValue("@packname", "pack" + lastId);
                    cmdid.Parameters.AddWithValue("@id", lastId);
                    cmdid.ExecuteNonQuery();

                    foreach (var card in cardscreatelist)
                    {
                        string cardstring = "{" + card + "}";
                        Cards cardjson = JsonSerializer.Deserialize<Cards>(cardstring);
                        using var cmdiP = new NpgsqlCommand("INSERT INTO card (id, name, damage, owner) VALUES (@id, @name, @damage, @owner)", conn);
                        cmdiP.Parameters.AddWithValue("@id", cardjson.Id);
                        cmdiP.Parameters.AddWithValue("@name", cardjson.Name);
                        cmdiP.Parameters.AddWithValue("@damage", cardjson.Damage);
                        cmdiP.Parameters.AddWithValue("@owner", "pack" + lastId);

                        var worked = cmdiP.ExecuteNonQuery();
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
                conn.Close();
            }
        }

    }
}