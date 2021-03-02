using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TicTacToe_Server
{
	static class RoomManager
	{
		static List<Room> rooms = new List<Room>();
		static int lastRoomId = 0;
		public static Message CreateRoom() 
		{
			int id = ++lastRoomId; // Can be replaced with id generation algorithm
			Room room = new Room(id);
			rooms.Add(room);
			return new Message() { Type = MessageType.RoomCreated.ToString(), Data = room.RoomName };
		}
		public static Message CreateRoom(out Room room)
		{
			int id = ++lastRoomId;
			room = new Room(id);
			rooms.Add(room);
			return new Message() { Type = MessageType.RoomCreated.ToString(), Data = room.RoomName };
		}
		public static Message CreateRoom(Player player)
		{
			int id = ++lastRoomId;
			Room room = new Room(id);
			room.Join(player);
			rooms.Add(room);
			return new Message() { Type = MessageType.RoomCreated.ToString(), Data = room.RoomName };

		}
		public static Message CreateRoom(Player player, out Room room)
		{
			int id = ++lastRoomId;
			room = new Room(id);
			room.Join(player);
			rooms.Add(room);
			return new Message() { Type = MessageType.RoomCreated.ToString(), Data = room.RoomName };

		}

		public static Message JoinRoom(Player player, Room room)
		{
			if (room != null)
			{
				return room.Join(player);
			}
			else
			{
				return new Message() { Type = MessageType.Error.ToString(), Data = "Invalid Room name" };
			}
		}

		public static Message JoinRoom(Player player, string roomName) 
		{
			Room room = rooms.Where(x => x.RoomName == roomName).FirstOrDefault();
			if (room != null)
			{
				return room.Join(player);
			}
			else
			{
				return new Message() { Type = MessageType.Error.ToString(), Data = "Invalid Room name" };
			}
		}
		public static void LeaveRoom(Player player)
		{
			if (player.Room != null)
			{
				player.Room.Leave(player);
				if (player.Room.IsEmpty)
					CloseRoom(player.Room);
			}
		}
		public static void LeaveRoom(Player player, Room room)
		{
			if (room != null)
			{
				room.Leave(player);
			}
		}
		public static void LeaveRoom(Player player, string roomName)
		{
			Room room = rooms.Where(x => x.RoomName == roomName).FirstOrDefault();
			if (room != null)
			{
				room.Leave(player);
			}
		}

		public static void CloseRoom(Room room) 
		{
			rooms.Remove(room);
			room.Player1.Room = null;
			room.Player2.Room = null;
		}

	}
}
