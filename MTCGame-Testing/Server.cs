using MTCGame.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame_Testing
{
    internal class Servertest
    {
        [Test]
        public void missingPathShouldLeadTo404()
        {
            var server = new Server();
        }
    }
}

//public void RequestHandle(TcpClient client, Byte[] bytes, String? data, List<string> Tokens, Lobby Lobbylist)