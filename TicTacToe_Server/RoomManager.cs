using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TicTacToe_Server
{
	static class RoomManager
	{
		static List<Room> rooms = new List<Room>();

		public static Message CreateRoom() 
		{
			int id = rooms.Count + 1;
			Room room = new Room(id);
			rooms.Add(room);
			return new Message() { Type = "RoomCreated", Data = room.RoomName };
		}
		public static Message CreateRoom(out Room room)
		{
			int id = rooms.Count + 1;
			room = new Room(id);
			rooms.Add(room);
			return new Message() { Type = "RoomCreated", Data = room.RoomName };
		}
		public static Message CreateRoom(Player player)
		{
			int id = rooms.Count + 1;
			Room room = new Room(id);
			room.Join(player);
			rooms.Add(room);
			return new Message() { Type = "RoomCreated", Data = room.RoomName };

		}
		public static Message CreateRoom(Player player, out Room room)
		{
			int id = rooms.Count + 1;
			room = new Room(id);
			room.Join(player);
			rooms.Add(room);
			return new Message() { Type = "RoomCreated", Data = room.RoomName };

		}

		public static Message JoinRoom(Player player, Room room)
		{
			if (room != null)
			{
				return room.Join(player);
			}
			else
			{
				return new Message() { Type = "Error", Data = "Invalid Room name" };
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
				return new Message() { Type = "Error", Data = "Invalid Room name" };
			}
		}
		public static void LeaveRoom(Player player)
		{
			if (player.room != null)
			{
				player.room.Leave(player);
				if (player.room.IsEmpty)
					CloseRoom(player.room);
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
		}

	}
}
