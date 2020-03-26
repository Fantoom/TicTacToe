using System;
using System.Threading;
using Newtonsoft.Json;
namespace TicTacToe_Client
{
	class Program
	{
		static void Main(string[] args)
		{
			Client client = new Client();
			client.StartClient();
			/*new Thread(() =>
			{
				//Thread.CurrentThread.IsBackground = true;
				
				client.StartClient();
			}).Start();
			Thread.Sleep(100);
			*/
			Console.WriteLine("Not Blocked");
			client.Send(JsonConvert.SerializeObject(new Message() { Type = "CreateRoom", Data = "" }));
		}
		
	}
}
