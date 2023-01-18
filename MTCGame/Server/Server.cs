using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json;
using System.Threading;
using System.Net.Mime;
using MTCGame.Database;
using MTCGame.Battlelogic;

namespace MTCGame.Server;

public class Server
{
    public void StartServer()
    {
        TcpListener? server;
        try
        {
            // Set the TcpListener on port 13000.
            Int32 port = 10001;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[4096];
            string? data = null;

            List<string>? Tokens = new List<string>();

            Lobby Lobbylist = new Lobby();
            // Enter the listening loop.
            while (true)
            {
                Console.Write("Waiting for a connection... ");

                TcpClient client = server.AcceptTcpClient();
                Thread thread = new Thread(() => RequestHandle(client, bytes, data, Tokens, Lobbylist));
                thread.Start();
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine($"SocketException: {e}");
        }
        finally
        {
            //server.Stop();
        }

        Console.WriteLine($"{Environment.NewLine}Hit enter to continue...");
        Console.Read();
    }

    public void RequestHandle(TcpClient client, Byte[] bytes, String? data, List<string> Tokens, Lobby Lobbylist)
    {
        Console.WriteLine("Connected!");

        data = null;

        // Get a stream object for reading and writing
        NetworkStream stream = client.GetStream();

        int i;

        string? UserAuthorization = null;
        string? Authorization = null;
        string? UserPlayer = null;
        // Loop to receive all the data sent by the client.
        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            // Translate data bytes to a ASCII string.
            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            Console.WriteLine($"Received:{Environment.NewLine}{data}");

            // Process the data sent by the client.
            //data = data.ToUpper();
            Console.WriteLine("*************************************");
            string[] dataparsed = data.Split(" ", 3);
            string Method = dataparsed[0];
            string Path = dataparsed[1];
            Console.WriteLine($"Method: {Method}");
            Console.WriteLine($"Path: {Path}");
            int counterdata = 0;
            string[] authorizationparsed = dataparsed[2].Split($"{Environment.NewLine}");
            counterdata = 0;

            foreach (var stringparsed in authorizationparsed)
            {
                if (stringparsed.Length > 13)
                {
                    //Console.WriteLine($"{counterdata}: {stringparsed}");
                    string authsub = stringparsed.Substring(0, 14);
                    counterdata++;
                    if (authsub.Contains("Authorization:"))
                    {
                        string[] token = stringparsed.Split(" ", 3);
                        UserAuthorization = token[2];
                        string[] userauth = UserAuthorization.Split("-");
                        UserPlayer = userauth[0];
                        Authorization = userauth[1];
                        Console.WriteLine($"User: {UserPlayer}");
                        Console.WriteLine($"Authorizations: {Authorization}");
                    }
                }
            }

            Console.WriteLine("*************************************");
            if (Path.StartsWith("/users"))
            {
                string[] separatorsch = { "{", "}" };
                var infoparsed = dataparsed[2].Split(separatorsch, StringSplitOptions.RemoveEmptyEntries);
                string? info = null;
                if (infoparsed.Length > 1)
                {
                    info = infoparsed[1];
                    Console.WriteLine($"Info: {info}");
                }
                string jsonString = "{" + info + "}";
                if (Method == "POST")
                {
                    Login login = JsonSerializer.Deserialize<Login>(jsonString);
                    var cuser = new CreateUser(login.Username, login.Password);
                    data = cuser.Answer;
                }
                else if (Method == "GET")
                {
                    string[]? user = Path.Split("/");
                    if (user[2].Equals(UserPlayer))
                    {
                        var getbio = new ReadBio(UserPlayer);
                        data = getbio.Answer;
                    }
                    else data = "401|Access token is missing or invalid|Access token is missing or invalid";
                }
                else if (Method == "PUT")
                {
                    string[]? user = Path.Split("/");
                    if (user[2].Equals(UserPlayer))
                    {
                        var updbio = new UpdateBio(UserPlayer, jsonString);
                        data = updbio.Answer;
                    }
                    else data = "401|Access token is missing or invalid|Access token is missing or invalid";
                }
            }
            else if (Path == "/transactions/packages")
            {
                if (Tokens.Contains(UserPlayer + "-" + Authorization))
                {
                    var buyp = new BuyPackage(UserPlayer);
                    data = buyp.Answer;
                }
                else data = "401|Access token is missing or invalid|You are not authorized, please login!";
            }
            else if (Path == "/packages")
            {
                if (Tokens.Contains("admin-" + Authorization))
                {
                    string[] separatoreck = { "[", "]" };
                    var packagesparsed = dataparsed[2].Split(separatoreck, StringSplitOptions.RemoveEmptyEntries);
                    if (packagesparsed.Length > 1)
                    {
                        var packages = new CreatePackage(packagesparsed[1]);
                        data = packages.Answer;
                    }
                    else
                    {
                        Console.WriteLine("No Info transferred!");
                    }
                }
                else data = "403|Provided user is not admin|You are not authorized to add packages!";
            }
            else if (Path == "/sessions")
            {
                //Thread.Sleep(10000);
                string[] separatorsch = { "{", "}" };
                var infoparsed = dataparsed[2].Split(separatorsch, StringSplitOptions.RemoveEmptyEntries);
                string? info = null;
                if (infoparsed.Length > 1)
                {
                    info = infoparsed[1];
                    Console.WriteLine($"Info: {info}");
                }
                else
                {
                    Console.WriteLine("No Info transferred!");
                }

                string jsonString = "{" + info + "}";
                Login login = JsonSerializer.Deserialize<Login>(jsonString);
                var cuser = new LoginUser(login.Username, login.Password);

                string Token = login.Username + "-mtcgToken";

                var tempAnswer = cuser.Answer;
                var tempToken = tempAnswer.Split("|");

                if (string.Equals(tempToken[2], Token) && !Tokens.Contains(Token))
                {
                    Tokens.Add(Token);
                }

                foreach (var Tok in Tokens)
                {
                    Console.WriteLine($"Tokentest: {Tok}");
                }

                data = cuser.Answer;
            }
            else if (Path == "/cards")
            {
                if (Tokens.Contains(UserPlayer + "-" + Authorization))
                {
                    var cardlist = new PrintAllPlayerCards(UserPlayer);
                    data = cardlist.Answer;
                }
                else data = "401|Access token is missing or invalid|You are not authorized, please login!";
            }
            else if (Path == "/deck")
            {
                if (Tokens.Contains(UserPlayer + "-" + Authorization))
                {
                    if (Method == "GET")
                    {
                        var deckget = new PrintDeck(UserPlayer);
                        data = deckget.Answer;
                    }
                    else if (Method == "PUT")
                    {
                        var deckput = new ConfigureDeck(UserPlayer, dataparsed[2]);
                        data = deckput.Answer;
                    }
                }
                else data = "401|Access token is missing or invalid|You are not authorized, please login!";
            }
            else if (Path == "/stats")
            {
                if (Tokens.Contains(UserPlayer + "-" + Authorization))
                {
                    var stats = new ReadStats(UserPlayer);
                    data = stats.Answer;
                }
                else data = "401|Access token is missing or invalid|You are not authorized, please login!";
            }
            else if (Path == "/score")
            {
                if (Tokens.Contains(UserPlayer + "-" + Authorization))
                {
                    var score = new ReadScore(UserPlayer);
                    data = score.Answer;
                }
                else data = "401|Access token is missing or invalid|You are not authorized, please login!";
            }
            else if (Path == "/battles")
            {
                Lobbylist.AddUser(UserPlayer);
                while(Lobbylist.Battlelog == null) Thread.Sleep(500);
                data = Lobbylist.Battlelog;
            }
            else if (Path == "/tradings")
            {

            }
            else
            {
                data = "404|Not found|Path does not exist";
            }

            Console.WriteLine("#############################");
            foreach (var Toki in Tokens)
            {
                Console.WriteLine(Toki);
            }

            Console.WriteLine("#############################");

            string[] codeMessage = data.Split("|");
            string status = codeMessage[0];
            string statusMessage = codeMessage[1];

            data = "HTTP/1.1 " + status + " " + statusMessage + "\n";
            string payload = codeMessage[2];
            data += $"Content-Length: {payload.Length}\n";
            data += "Content-Type: application/json\n\n";
            if (payload != null)
                data += payload;

            byte[]? msg = null;
            if (data == null)
            {
                Console.WriteLine("An error occurred, data = null!");
            }
            else
            {
                msg = System.Text.Encoding.ASCII.GetBytes(data);
            }
            stream.Write(msg, 0, msg.Length);
            Console.WriteLine($"Sent:{Environment.NewLine}{data}{Environment.NewLine}");
        }
    }
}