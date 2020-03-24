using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe_Server
{
	class Room
	{

		private Player player1 = null;
		private Player player2 = null;
		public Player Player1 { get { return player1; } }
		public Player Player2 { get { return player2; } }
		public int roomId { get; }
		
		public string RoomName { get { return "Room_" + roomId.ToString(); } } 
		public Room(int id)
		{
			roomId = roomId;
		}

		public Message Join(Player player) 
		{
			if (player1 == null)
			{
				player1 = player;
				return new Message() { Type = "Succsses", Data = "Joined as Player1" };
			}
			else if (player2 == null)
			{
				player2 = player;
				return new Message() { Type = "Succsses", Data = "Joined as Player2" };
			}
			else 
			{
			 return new Message() { Type = "Error", Data = "Room is full"};
			}
		}

		public void Leave(Player player)
		{
			if (player1.playerId == player.playerId)
			{
				player1 = null;
			}
			else if (player2.playerId == player.playerId)
			{
				player2 = null;
			}
		}

	}
}
