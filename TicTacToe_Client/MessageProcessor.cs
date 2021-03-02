using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe_Client;
namespace TicTacToe_Client
{
	static class MessageProcessor
	{
		public delegate void OnMessageProcessed(string data);
		public static event OnMessageProcessed OnRoomCreated;
		public static event OnMessageProcessed OnJoined;
		public static event OnMessageProcessed OnMoved;


		public static void CreateRoom(Client client) 
		{
			string json = JsonConvert.SerializeObject(new Message() {Type = MessageType.CreateRoom.ToString(), Data = "" });
			client.SendAsync(json);
		}
		public static void JoinRoom(Client client, string RoomID)
		{
			string json = JsonConvert.SerializeObject(new Message() { Type = MessageType.JoinRoom.ToString(), Data = RoomID });
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

			switch (Enum.Parse<MessageType>(msg.Type.ToLower(), true))
			{
				case MessageType.CreateRoom:
					OnRoomCreated(msg.Data);
					break;
				case MessageType.Joined:
					OnJoined(msg.Data);
					break;
				case MessageType.Moved:
					OnMoved(msg.Data);
					break;
				case MessageType.Error:
					break;
				default:
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
}
