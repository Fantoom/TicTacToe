using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe_Server
{
	class Game
	{
		private Room room;
		public Player P1 { get { return room.Player1; } }
		public Player P2 { get { return room.Player2; } }

		private InGamePlayer currentPlayer;
		private InGamePlayer nextPlayer;
		private InGamePlayer inGameP1 = new InGamePlayer();
		private InGamePlayer inGameP2 = new InGamePlayer();

		private Dictionary<PlayerType,InGamePlayer> InGamePlayerList;

		private DeskCell[,] desk = new DeskCell[3, 3];

		public void Start()
		{
			//InGamePlayerList = new InGamePlayer[] { InGameP1, InGameP2 };
			inGameP1.player = P1;
			inGameP2.player = P2;
			desk = new DeskCell[3, 3];

			inGameP1.type = (PlayerType)typeof(PlayerType).GetRandomEnumValue();
			inGameP2.type = (PlayerType)(1 - (int)inGameP1.type);
			MessageProcessor.SendMessageToPlayer(inGameP1.player, new Message() { Type = MessageType.GameStart.ToString(), Data = $"Type:{inGameP1.type}" });
			MessageProcessor.SendMessageToPlayer(inGameP2.player, new Message() { Type = MessageType.GameStart.ToString(), Data = $"Type:{inGameP2.type}" });


			InGamePlayerList.Add(inGameP1.type, inGameP1);
			InGamePlayerList.Add(inGameP2.type, inGameP2);

			currentPlayer = InGamePlayerList[(PlayerType)typeof(PlayerType).GetRandomEnumValue()];
			nextPlayer = InGamePlayerList.Values.Where(x => x.player.PlayerId != currentPlayer.player.PlayerId).FirstOrDefault();

			MessageProcessor.SendMessageToPlayer(currentPlayer.player, new Message() { Type = MessageType.Turn.ToString(), Data = "true" });
			MessageProcessor.SendMessageToPlayer(nextPlayer.player, new Message() { Type = MessageType.Turn.ToString(), Data = "false" });

			//TODO Send message to players that game started and his type
		}

		public Message Move(Player player, int x , int y) 
		{
			Message answer = new Message() { Type = MessageType.Error.ToString(), Data = "Unknow error" };
			if (player.PlayerId == currentPlayer.player.PlayerId)
			{
				if (desk[y, x] == DeskCell.Empty)
				{
					desk[y, x] = (DeskCell)Enum.Parse(typeof(DeskCell), currentPlayer.type.ToString());
					string deskString = JsonConvert.SerializeObject(desk); //DeskToString(desk);
					MessageProcessor.SendMessageToPlayer(nextPlayer.player, new Message() { Type = MessageType.Desk.ToString(), Data = deskString });
					MessageProcessor.SendMessageToPlayer(currentPlayer.player, new Message() { Type = MessageType.Desk.ToString(), Data = deskString });


					answer = new Message() { Type = MessageType.Moved.ToString(), Data = $"{x},{y}" };
					CheckWin();

				}
			}
			else 
			{
				answer = new Message() { Type = MessageType.Error.ToString(), Data = "Not your turn" };
			}
			return answer;
		}

		private void CheckWin()
		{
			foreach (PlayerType type in (PlayerType[])Enum.GetValues(typeof(PlayerType)))
			{
				var castedType = (DeskCell)Enum.Parse(typeof(DeskCell), type.ToString());
				var player = InGamePlayerList[type];
				for (int y = 0; y < 3; y++)
				{
					if (new[] { desk[y, 0], desk[y, 1], desk[y, 2] }.All(x => x == castedType))
					{
						player.score++;
						MessageProcessor.SendMessageToPlayer(player.player, new Message() { Type = MessageType.Win.ToString(), Data = $"Score:{player.score}" });
					}
				}

				for (int x = 0; x < 3; x++)
				{
					if (new[] { desk[0, x], desk[1, x], desk[2, x] }.All(x => x == castedType))
					{
						player.score++;
						MessageProcessor.SendMessageToPlayer(player.player, new Message() { Type = MessageType.Win.ToString(), Data = $"Score:{player.score}" });
					}
				}
				if (new[] { desk[0, 0], desk[1, 1], desk[2, 2] }.All(x => x == castedType))
				{
					player.score++;
					MessageProcessor.SendMessageToPlayer(player.player, new Message() { Type = MessageType.Win.ToString(), Data = $"Score:{player.score}" });
				}
				if (new[] { desk[0, 2], desk[1, 1], desk[2, 0] }.All(x => x == castedType))
				{
					InGamePlayerList[type].score++;
					MessageProcessor.SendMessageToPlayer(player.player, new Message() { Type = MessageType.Win.ToString(), Data = $"Score:{player.score}" });
				}
			}
		}
		private string DeskToString(DeskCell[,] desk)
		{
			string readyString = "";
			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					readyString += $"{x},{y}:{desk[y,x]}|";
				}
			}
			return readyString;
		}

		private void SwitchPlayer()
		{
			var temp = currentPlayer;
			currentPlayer = nextPlayer;
			nextPlayer = currentPlayer;

			MessageProcessor.SendMessageToPlayer(currentPlayer.player, new Message() { Type = MessageType.Turn.ToString(), Data = "True" });
			MessageProcessor.SendMessageToPlayer(nextPlayer.player, new Message() { Type = MessageType.Turn.ToString(), Data = "False" });

		}

		class InGamePlayer 
		{
			public Player player;
			public PlayerType type;
			public int score;
		}
		enum DeskCell 
		{
			Empty = 0,
			X = 1,
			O = 2
		}
		enum PlayerType
		{
			X = 0,
			O = 1
		}

	}
	public static class EnumExtensions
	{
		public static Enum GetRandomEnumValue(this Type t)
		{
			return Enum.GetValues(t)          // get values from Type provided
				.OfType<Enum>()               // casts to Enum
				.OrderBy(e => Guid.NewGuid()) // mess with order of results
				.FirstOrDefault();            // take first item in result
		}
	}
}
