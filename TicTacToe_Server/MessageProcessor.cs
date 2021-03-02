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
				return new Message() { Type = MessageType.Error.ToString(), Data = "Invalid Json or Server-side error" };
			}

			Message answerMessage = new Message();

			switch (Enum.Parse<MessageType>(msg.Type.ToLower(), true))
			{
				case MessageType.CreateRoom:
					 answerMessage = RoomManager.CreateRoom(player);
					break;
				case MessageType.JoinRoom:
					 answerMessage = RoomManager.JoinRoom(player,msg.Data);
					break;
				case MessageType.LeaveRoom:
					 RoomManager.LeaveRoom(player, msg.Data);
					break;
				case MessageType.Move:
					answerMessage = GameManager.MakeMove(player, msg.Data);
					break;
				default:
					answerMessage = new Message() { Type = MessageType.Error.ToString(), Data = "Unknow type" };
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
				player.Session.SendAsync(json);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
	public enum MessageType 
	{
		CreateRoom,
		JoinRoom,
		LeaveRoom,
		Move,
		RoomCreated,
		Error,
		Moved,
		PublicModeSwitch,
		Joined,
		Turn,
		Desk,
		Win,
		GameStart
	}
}
