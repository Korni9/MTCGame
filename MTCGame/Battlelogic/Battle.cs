using System.Runtime.CompilerServices;
using System.Text.Json;
using MTCGame.Database;

namespace MTCGame.Battlelogic
{
    public class Battle
    {

        public string? Answer;
        private Random _rng = new();
        private string _user1;
        private string _user2;

        public Battle(string user1, string user2)
        {
            _user1 = user1;
            _user2 = user2;

            Console.WriteLine("Starting battle...");

            var Deck1 = new RetrieveDeck(user1);
            Console.WriteLine(Deck1.Deckjson);
            var cardList1 = JsonSerializer.Deserialize<List<Cards>>(Deck1.Deckjson);

            foreach (var card in cardList1)
            {
                if (!card.Name.Contains("Fire") && !card.Name.Contains("Water") && !card.Name.Contains("Regular"))
                {
                    card.Name = $"Regular{card.Name}";
                }
                Console.WriteLine("card.Id: " + card.Id);
                Console.WriteLine("card.Name: " + card.Name);
                Console.WriteLine("card.Damage: " + card.Damage);
            }

            var Deck2 = new RetrieveDeck(user2);
            Console.WriteLine(Deck2.Deckjson);
            var cardList2 = JsonSerializer.Deserialize<List<Cards>>(Deck2.Deckjson);

            foreach (var card in cardList2)
            {
                if (!card.Name.Contains("Fire") && !card.Name.Contains("Water") && !card.Name.Contains("Regular"))
                {
                    card.Name = $"Regular{card.Name}";
                }
                Console.WriteLine("card.Id: " + card.Id);
                Console.WriteLine("card.Name: " + card.Name);
                Console.WriteLine("card.Damage: " + card.Damage);
            }

            Answer = $"999|Testing|Deck1:{Deck1.Deckjson}\n Deck2:{Deck2.Deckjson}\n";

            Console.WriteLine($"Battle has started between {_user1} and {_user2}!");
            int round = 1;
            while ((cardList1.Count != 0 || cardList2.Count != 0) && round < 100 )
            {
                int randomNumberuser1 = _rng.Next(cardList1.Count);
                int randomNumberuser2 = _rng.Next(cardList2.Count);

                

                if (!cardList1[randomNumberuser1].Name.Contains("Spell") &&
                         !cardList2[randomNumberuser2].Name.Contains("Spell"))
                {
                    if (cardList1[randomNumberuser1].Damage > cardList2[randomNumberuser2].Damage)
                    {
                        Console.WriteLine($"{cardList1[randomNumberuser1].Name} has Won the fight!\n");
                        cardList1.Add(cardList2[randomNumberuser2]);
                        cardList2.Remove(cardList2[randomNumberuser2]);
                    }
                    else if (cardList1[randomNumberuser1].Damage < cardList2[randomNumberuser2].Damage)
                    {
                        Console.WriteLine($"{cardList2[randomNumberuser2].Name} has Won the fight!\n");
                        cardList2.Add(cardList1[randomNumberuser1]);
                        cardList1.Remove(cardList1[randomNumberuser1]);
                    }
                    else
                    {
                        Console.WriteLine("MONSTER MONSTER both Cards have the same Damage, No winner!\n");
                    }
                }
                else
                { 
                    if (cardList1[randomNumberuser1].Name.Contains("Fire") &&
                        cardList2[randomNumberuser2].Name.Contains("Fire") ||
                        cardList1[randomNumberuser1].Name.Contains("Water") &&
                        cardList2[randomNumberuser2].Name.Contains("Water") ||
                        cardList1[randomNumberuser1].Name.Contains("Regular") &&
                        cardList2[randomNumberuser2].Name.Contains("Regular"))
                    {
                        Console.WriteLine($"Both Cards have same Element, Damage is not modified!"); 
                        if (cardList1[randomNumberuser1].Damage > cardList2[randomNumberuser2].Damage)
                        {
                            Console.WriteLine($"{cardList1[randomNumberuser1].Name} has Won the fight!\n"); 
                            cardList1.Add(cardList2[randomNumberuser2]); 
                            cardList2.Remove(cardList2[randomNumberuser2]);
                        }
                        else if (cardList1[randomNumberuser1].Damage < cardList2[randomNumberuser2].Damage) 
                        {
                            Console.WriteLine($"{cardList2[randomNumberuser2].Name} has Won the fight!\n"); 
                            cardList2.Add(cardList1[randomNumberuser1]);
                            cardList1.Remove(cardList1[randomNumberuser1]);
                        }
                        else
                        { 
                            Console.WriteLine("SPELL SPELL both Cards have the same Damage, No winner!\n");
                        }
                    }
                    else if ((cardList1[randomNumberuser1].Name.Contains("Fire") &&
                              cardList2[randomNumberuser2].Name.Contains("Regular")) ||
                             (cardList1[randomNumberuser1].Name.Contains("Regular") &&
                              cardList2[randomNumberuser2].Name.Contains("Water")) ||
                             (cardList1[randomNumberuser1].Name.Contains("Water") &&
                              cardList2[randomNumberuser2].Name.Contains("Fire")))
                    {
                        Console.WriteLine(
                            $"{cardList1[randomNumberuser1].Name} is strong against {cardList2[randomNumberuser2].Name}, Damage is doubled to {cardList1[randomNumberuser1].Damage * 2}.");
                        Console.WriteLine(
                            $"{cardList2[randomNumberuser2].Name} is weak against {cardList1[randomNumberuser1].Name}, Damage is halved to {cardList2[randomNumberuser2].Damage / 2}.");
                        if (cardList1[randomNumberuser1].Damage * 2 > cardList2[randomNumberuser2].Damage / 2)
                        {
                            Console.WriteLine($"{cardList1[randomNumberuser1].Name} has Won the fight!\n");
                            cardList1.Add(cardList2[randomNumberuser2]);
                            cardList2.Remove(cardList2[randomNumberuser2]);
                        }
                        else if (cardList1[randomNumberuser1].Damage * 2 < cardList2[randomNumberuser2].Damage / 2)
                        {
                            Console.WriteLine($"{cardList2[randomNumberuser2].Name} has Won the fight!\n");
                            cardList2.Add(cardList1[randomNumberuser1]);
                            cardList1.Remove(cardList1[randomNumberuser1]);
                        }
                        else
                        {
                            Console.WriteLine("STRONG both Cards have the same Damage, No winner!\n");
                        }
                    }

                    else if ((cardList1[randomNumberuser1].Name.Contains("Regular") &&
                              cardList2[randomNumberuser2].Name.Contains("Fire")) ||
                             (cardList1[randomNumberuser1].Name.Contains("Water") &&
                              cardList2[randomNumberuser2].Name.Contains("Regular")) ||
                             (cardList1[randomNumberuser1].Name.Contains("Fire") &&
                              cardList2[randomNumberuser2].Name.Contains("Water")))
                    {
                        Console.WriteLine(
                            $"{cardList1[randomNumberuser1].Name} is weak against {cardList2[randomNumberuser2].Name}, Damage is halved to {cardList1[randomNumberuser1].Damage / 2}.");
                        Console.WriteLine(
                            $"{cardList2[randomNumberuser2].Name} is strong against {cardList1[randomNumberuser1].Name}, Damage is doubled to {cardList2[randomNumberuser2].Damage * 2}.");
                        if (cardList1[randomNumberuser1].Damage / 2 > cardList2[randomNumberuser2].Damage * 2)
                        {
                            Console.WriteLine($"{cardList1[randomNumberuser1].Name} has Won the fight\n!");
                            cardList1.Add(cardList2[randomNumberuser2]);
                            cardList2.Remove(cardList2[randomNumberuser2]);
                        }
                        else if (cardList1[randomNumberuser1].Damage / 2 < cardList2[randomNumberuser2].Damage * 2)
                        {
                            Console.WriteLine($"{cardList2[randomNumberuser2].Name} has Won the fight!\n");
                            cardList2.Add(cardList1[randomNumberuser1]);
                            cardList1.Remove(cardList1[randomNumberuser1]);
                        }
                        else
                        {
                            Console.WriteLine("WEAK both Cards have the same Damage, No winner!\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR Check code!");
                    }
                }

                Console.WriteLine($"XXXXXXXXXXXXXXXXX {round} XXXXXXXXXXXXXXXXXXXX");
                int counter = 1;
                int counter2 = 1;
                Console.WriteLine(user1);
                foreach (var card in cardList1)
                {

                    Console.WriteLine(counter + ": " + card.Name);
                    counter++;
                }

                Console.WriteLine(user2);
                foreach (var card in cardList2)
                {
                    Console.WriteLine(counter2 + ": " + card.Name);
                    counter2++;
                }
                Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                round++;
            }

            if (cardList1.Count == 0)
            {
                Answer = _user2;
            }
            else if (cardList2.Count == 0)
            {
                Answer = _user1;
            }
            else
            {
                Answer = "200|No winner exists!|No winner exists!";
            }
        }
    }
}
         



