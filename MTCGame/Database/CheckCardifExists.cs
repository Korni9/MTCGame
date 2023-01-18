using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MTCGame.Server;
using Npgsql;

namespace MTCGame.Database
{
    public class checkCardifExists
    {
        public bool ifExists = false;
        public checkCardifExists(string card, NpgsqlConnection conn)
        {

            string cardstring = "{" + card + "}";
            Console.WriteLine(cardstring);
            Cards cardjs = JsonSerializer.Deserialize<Cards>(cardstring);

            Console.WriteLine($"CardID: {cardjs.Id} CardName: {cardjs.Name}");
            string sqlcheckId = "SELECT COUNT(*) FROM card WHERE id = @id";
            using (var cmdcheckE = new NpgsqlCommand(sqlcheckId, conn))
            {
                cmdcheckE.Parameters.AddWithValue("@id", cardjs.Id);
                long count = (long)cmdcheckE.ExecuteScalar();
                if (count > 0) ifExists = true;
                else ifExists = false;
            }
        }
    }
}