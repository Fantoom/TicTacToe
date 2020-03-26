using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using System.Linq;

namespace TicTacToe_Server
{
	class Server : TcpServer
    {


        private List<Player> players = new List<Player>();

        public static Server instance { get; private set; }

        public Server(IPAddress address, int port) : base(address, port) { }

        protected override TcpSession CreateSession() { return new PlayerSession(this); }

        protected override void OnConnected(TcpSession session)
        {
            Player player = new Player(session);
            players.Add(player);
            ((PlayerSession)session).Player = player;
        }

        protected override void OnDisconnected(TcpSession session)
        {
            // base.OnDisconnected(session);
            players.RemoveAll(x => x.playerId == session.Id);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP server caught an error with code {error}");
        }

    }

    class PlayerSession : TcpSession
    {
        
        public Player Player { get; set; }

        public PlayerSession(TcpServer server) : base(server) { }
        
        protected override void OnConnected()
        {
            Console.WriteLine($"Chat TCP session with Id {Id} connected!");

            // Send invite message
           // string message = "Hello from TCP chat! Please send a message or '!' to disconnect the client!";
           // SendAsync(message);
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Chat TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Console.WriteLine("Incoming: " + message);

            MessageProcessor.Process(message, Player);

            // If the buffer starts with '!' the disconnect the current session
            if (message == "!")
                Disconnect();
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP session caught an error with code {error}");
        }
    }

}
