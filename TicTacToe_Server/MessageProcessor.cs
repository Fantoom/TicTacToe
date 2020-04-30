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

			switch (msg.Type.ToLower())
			{
				case "createroom":
					 answerMessage = RoomManager.CreateRoom(player);
					break;
				case "joinroom":
					 answerMessage = RoomManager.JoinRoom(player,msg.Data);
					break;
				case "leaveroom":
					 RoomManager.LeaveRoom(player, msg.Data);
					break;
				case "move":
					answerMessage = GameManager.MakeMove(player, msg.Data);

					break;
				default:
					answerMessage = new Message() { Type = "Error", Data = "Unknow type" };
					break;
			}
			SendMessageToPlayer(player, answerMessage);
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
		public static void SendMessageToPlayer(Player player, Message message) 
		{
			try
			{
				string json = JsonConvert.SerializeObject(message);
				//Server.instance.Send(player.client.socket, json);
				player.session.SendAsync(json);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
	
}
