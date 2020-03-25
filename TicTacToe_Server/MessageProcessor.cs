using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace TicTacToe_Server
{
	static class MessageProcessor
	{
		public static Message Process(string message , Player player)
		{
			var msg = ConvertMessage(message);

			if (msg == null)
			{ 
				return new Message() { Type = "Error", Data = "Invalid Json or Server-side error" };
			}

			Message answerMessage = new Message();

			switch (msg.Type)
			{
				case "CreateRoom":
					 answerMessage = RoomManager.CreateRoom(player);
					break;
				case "JoinRoom":
					 answerMessage = RoomManager.JoinRoom(player,msg.Data);
					break;
				case "LeaveRoom":
					 RoomManager.LeaveRoom(player, msg.Data);
					break;
				case "Move":
					 //TODO: add logic for doing move
					break;
				default:
					answerMessage = new Message() { Type = "Error", Data = "Unknow type" };
					break;
			}

			return answerMessage;
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
