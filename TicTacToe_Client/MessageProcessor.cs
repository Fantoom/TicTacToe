using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe_Client;
namespace TicTacToe_Client
{
	static class MessageProcessor
	{
		public static void CreateRoom(Client client) 
		{
			string json = JsonConvert.SerializeObject(new Message() {Type = "CreateRoom", Data = "" });
			client.SendAsync(json);
		}
		public static void JoinRoom(Client client, string RoomID)
		{
			string json = JsonConvert.SerializeObject(new Message() { Type = "JoinRoom", Data = RoomID });
			client.SendAsync(json);
		}

		public static void Process(string message)
		{
			var msg = ConvertMessage(message);

			if (msg == null)
			{
				return;
			}

			Message answerMessage = new Message();

			switch (msg.Type)
			{
				case "RoomCreated":
					// TODO: add Game Window
					break;
				case "JoinRoom":
					// TODO: add Game Window
					break;
				case "Joined":
					// TODO: add Game Window
					break;
			
				default:
					answerMessage = new Message() { Type = "Error", Data = "Unknow type" };
					break;
			}

		}
		public static Message ConvertMessage(string message)
		{
			Message msg = null;
			try
			{
				msg = JsonConvert.DeserializeObject<Message>(message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			return msg;
		}
	}
}
