using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TicTacToe_Server
{
	static class RoomManager
	{
		static List<Room> rooms = new List<Room>();

		static Room CreateRoom() 
		{
			int id = rooms.Count + 1;
			Room room = new Room(id);
			rooms.Add(room);
			return room;
		}
		static Room CreateRoom(Player player)
		{
			int id = rooms.Count + 1;
			Room room = new Room(id);
			room.Join(player);
			rooms.Add(room);
			return room;
		}

		static Message JoinRoom(Player player, string roomName) 
		{
			Room room = rooms.Where(x => x.RoomName == roomName).FirstOrDefault();
			return room.Join(player);
		}

	}
}
