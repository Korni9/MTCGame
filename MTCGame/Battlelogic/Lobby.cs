using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MTCGame.Database;
using MTCGame.Server;

namespace MTCGame.Battlelogic
{
    public class Lobby
    {
        public List<string> _users;
        private int _maxUsers;
        public string? Battlelog { get; set; }

        public Lobby()
        {
            _users = new List<string>();
            _maxUsers = 2;
        }

        public void AddUser(string user)
        {
            _users.Add(user);
            Console.WriteLine($"{user} has joined the lobby.");
            if (_users.Count == _maxUsers)
            {
                var battle = new Battle(_users[0], _users[1]);
                Battlelog = battle.Answer;
                Console.WriteLine(Battlelog);
                _users.Clear();
                Console.WriteLine("Users have been removed from the lobby.");

                if (Battlelog.StartsWith("999") || Battlelog != null)
                {
                    Console.WriteLine("No Winner!");
                }
                else
                {
                    if (Battlelog.Equals(user))
                    {
                        Console.WriteLine("You have won!");
                        var uelo = new UpdateElo(Battlelog, user);
                    }
                    else
                    {
                        Console.WriteLine("You have lost!");
                        var uelo = new ReduceElo(user, Battlelog);
                    }
                }
            }
        }
    }
}