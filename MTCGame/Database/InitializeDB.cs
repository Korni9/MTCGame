using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MTCGame.Database
{
    internal class InitializeDB
    {
        public readonly string? Deckjson;
        public readonly bool Success;
        private static readonly string _Host = "127.0.0.1";
        private static readonly string _User = "postgres";
        private static readonly string _DBname = "MTCG";
        private static readonly string _Password = "12345678";
        private static readonly string _Port = "5432";

        public InitializeDB()
        {
            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText =
                        "DELETE from player; DELETE from deck; DELETE from card; DELETE from packs; SELECT setval('packs_id_seq', 1);";
                    var success = command.ExecuteNonQuery();
                    if(success > 0)Success = true;
                    else Success = false;
                }
                conn.Close();
            }
        }
    }
}
