using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using TcpClient = NetCoreServer.TcpClient;

namespace TicTacToe_Client
{
    public class Client : TcpClient
    {
        public delegate void OnDataRecived(string data);
        public delegate void OnMessageRecived(Message data);


        public event OnDataRecived onDataRecived = delegate { };
        public event OnMessageRecived onMessageRecived = delegate { };

        public Client(string address, int port) : base(address, port) { }

        public void DisconnectAndStop()
        {
            _stop = true;
            DisconnectAsync();
            while (IsConnected)
                Thread.Yield();
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"Game TCP client connected a new session with Id {Id}");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Game TCP client disconnected a session with Id {Id}");

            // Wait for a while...
            Thread.Sleep(1000);

            // Try to connect again
            if (!_stop)
                ConnectAsync();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string data = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            onDataRecived?.Invoke(data);
            try
            {
                Message message = JsonConvert.DeserializeObject<Message>(data);
                onMessageRecived(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            Console.WriteLine(data);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Game TCP client caught an error with code {error}");
        }

        private bool _stop;
    }
}
