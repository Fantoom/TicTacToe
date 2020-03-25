using System;
using Newtonsoft.Json;
namespace TicTacToe_Client
{
	class Program
	{
		static void Main(string[] args)
		{
			Client client = new Client();
			client.StartClient();
			client.Send(JsonConvert.SerializeObject(new Message() { Type = "CreateRoom", Data = "" }));
		}
		
	}
}
