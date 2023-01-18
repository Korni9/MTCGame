﻿using System;
using System.Text.Json;
using Npgsql;

namespace MTCGame.Database
{
    public class ReadScore   
    {
        public readonly string? Answer;
        private static readonly string _Host = "127.0.0.1";
        private static readonly string _User = "postgres";
        private static readonly string _DBname = "MTCG";
        private static readonly string _Password = "12345678";
        private static readonly string _Port = "5432";

        public ReadScore(string username)
        {
            string connString = $"Server={_Host}; User Id={_User}; Database={_DBname}; Port={_Port}; Password={_Password}; SSLMode=Prefer";

            using (var conn = new NpgsqlConnection(connString))
            {
                Console.WriteLine($"Opening connection, read Elo for user: {username}");
                conn.Open();
                string sql = $"SELECT name, wins, losses, elo FROM player ORDER BY wins";
                using (var command = new NpgsqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@player", username);
                    Answer = $"200|The stats could be retrieved successfully.|[";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        int wins = reader.GetInt32(1);
                        int losses = reader.GetInt32(2);
                        int elo = reader.GetInt32(3);
                        string jsonString = $"{{\"name\":\"{name}\",\"wins\":{wins},\"losses\":{losses},\"elo\":{elo}}}";
                        JsonElement json = JsonSerializer.Deserialize<JsonElement>(jsonString);
                        Answer += json +",";
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