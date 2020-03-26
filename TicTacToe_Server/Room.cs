using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe_Server
{
	class Room
	{
		public Player RoomOwner { get; private set; } = null;
		public Player Player1 { get; private set; } = null;
		public Player Player2 { get; private set; } = null;
		public int roomId { get; }

		public bool IsPublic { get; private set; } = true;

		public bool IsEmpty { get { return Player1 == null && Player2 == null && RoomOwner == null; }  }
		public bool IsFull  { get { return Player1 != null || Player2 != null; } }

		public string RoomName { get { return "Room_" + roomId.ToString(); } } 

		public Game game { get; private set; }

		public Room(int id)
		{
			roomId = roomId;
		}

		public Message Join(Player player) 
		{
			if (Player1 == null && Player2 == null && RoomOwner == null)
			{
				Player1 = player;
				RoomOwner = player;
				player.room = this;

				return new Message() { Type = "Joined", Data = "JoinedAsOwner" };
			}
			if (Player1 == null)
			{
				Player1 = player;
				player.room = this;
				return new Message() { Type = "Joined", Data = "JoinedAsP1" };
			}
			else if (Player2 == null)
			{
				Player2 = player;
				player.room = this;
				return new Message() { Type = "Joined", Data = "JoinedAsP2" };
			}
			else 
			{
			 return new Message() { Type = "Error", Data = "Room is full"};
			}
		}

		public void Leave(Player player)
		{
			
			if (Player1.playerId == player.playerId)
			{
				Player1 = null;
			}
			else if (Player2.playerId == player.playerId)
			{
				Player2 = null;
			}
			player.room = null;
			if (RoomOwner.playerId == player.playerId)
			{
				if (Player1 != null)
				{
					RoomOwner = Player1;
				}
				else if (Player2 != null)
				{
					RoomOwner = Player2;
				}
				else
				{
					RoomOwner = null;

					//RoomManager.CloseRoom();
				}
			}
		}

		public Message SwitchPublicMode(Player player)
		{
			if (RoomOwner.playerId == player.playerId)
			{
				IsPublic = !IsPublic;
				return new Message() {Type = "PublicModeSwitch", Data = "ok" };
			}
			else
			{
				return new Message() { Type = "Error", Data = "You are not room owner" };
			}
		}

	}
}
