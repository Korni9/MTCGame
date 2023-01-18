using System;
using MTCGame.Database;
using MTCGame.Server;

var init = new InitializeDB();
if (init.Success) Console.WriteLine("DB initialized successfully");
else Console.WriteLine("Error initializing DB");

var server = new Server();
server.StartServer();

