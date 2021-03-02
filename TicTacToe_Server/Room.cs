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
		public int RoomId { get; private set; }

		public bool IsPublic { get; private set; } = true;

		public bool IsEmpty { get { return Player1 == null && Player2 == null && RoomOwner == null; }  }
		public bool IsFull  { get { return Player1 != null || Player2 != null; } }

		public string RoomName { get { return "Room_" + RoomId.ToString(); } } 

		public Game Game { get; private set; }

		public Room(int id)
		{
			RoomId = RoomId;
		}

		public Message Join(Player player) 
		{
			if (Player1 == null && Player2 == null && RoomOwner == null)
			{
				Player1 = player;
				RoomOwner = player;
				player.Room = this;

				return new Message() { Type = MessageType.Joined.ToString(), Data = "JoinedAsOwner" };
			}
			if (Player1 == null)
			{
				Player1 = player;
				player.Room = this;
				return new Message() { Type = MessageType.Joined.ToString(), Data = "JoinedAsP1" };
			}
			else if (Player2 == null)
			{
				Player2 = player;
				player.Room = this;
				return new Message() { Type = MessageType.Joined.ToString(), Data = "JoinedAsP2" };
			}
			else 
			{
			 return new Message() { Type = MessageType.Error.ToString(), Data = "Room is full"};
			}
		}

		public void Leave(Player player)
		{
			
			if (Player1.PlayerId == player.PlayerId)
			{
				Player1 = null;
			}
			else if (Player2.PlayerId == player.PlayerId)
			{
				Player2 = null;
			}
			player.Room = null;
			if (RoomOwner.PlayerId == player.PlayerId)
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
			if (RoomOwner.PlayerId == player.PlayerId)
			{
				IsPublic = !IsPublic;
				return new Message() {Type = MessageType.PublicModeSwitch.ToString(), Data = "ok" };
			}
			else
			{
				return new Message() { Type = MessageType.Error.ToString(), Data = "You are not room owner" };
			}
		}

	}
}
