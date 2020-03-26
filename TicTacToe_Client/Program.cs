using System;
using System.Threading;
using Newtonsoft.Json;

namespace TicTacToe_Client
{
	class Program
	{
		static private string ip = "127.0.0.1";
		static private int port = 11000;
		static void Main(string[] args)
		{
			Client client = new Client(ip,port);
			
			Console.WriteLine("Not Blocked");
			Console.WriteLine($"TCP server address: {ip}");
			Console.WriteLine($"TCP server port: {port}");

			Console.WriteLine();

			// Create a new TCP chat client

			// Connect the client
			Console.Write("Client connecting...");
			client.ConnectAsync();
			Console.WriteLine("Done!");

			Console.WriteLine("Press Enter to stop the client or '!' to reconnect the client...");
			//Console.WriteLine(JsonConvert.SerializeObject(new Message() { Type = "CreateRoom", Data = "" }));
			//client.SendAsync(JsonConvert.SerializeObject(new Message() { Type = "CreateRoom", Data = "" }));
			for (; ; )
			{
				string line = Console.ReadLine();
				if (string.IsNullOrEmpty(line))
					break;

				// Disconnect the client
				if (line == "!")
				{
					Console.Write("Client disconnecting...");
					client.DisconnectAsync();
					Console.WriteLine("Done!");
					continue;
				}

				// Send the entered text to the chat server
				client.SendAsync(line);
			}
		}

	}
}
