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
		private InGamePlayer InGameP1 = new InGamePlayer();
		private InGamePlayer InGameP2 = new InGamePlayer();

		private Dictionary<PlayerType,InGamePlayer> InGamePlayerList;

		private DeskCell[,] desk = new DeskCell[3, 3];

		public void Start()
		{
			//InGamePlayerList = new InGamePlayer[] { InGameP1, InGameP2 };
			InGameP1.player = P1;
			InGameP2.player = P2;
			desk = new DeskCell[3, 3];

			InGameP1.type = (PlayerType)typeof(PlayerType).GetRandomEnumValue();
			InGameP2.type = (PlayerType)(1 - (int)InGameP1.type);
			MessageProcessor.SendMessageToPlayer(InGameP1.player, new Message() { Type = "GameStart", Data = $"Type:{InGameP1.type.ToString()}" });
			MessageProcessor.SendMessageToPlayer(InGameP2.player, new Message() { Type = "GameStart", Data = $"Type:{InGameP2.type.ToString()}" });


			InGamePlayerList.Add(InGameP1.type, InGameP1);
			InGamePlayerList.Add(InGameP2.type, InGameP2);

			currentPlayer = InGamePlayerList[(PlayerType)typeof(PlayerType).GetRandomEnumValue()];
			nextPlayer = InGamePlayerList.Values.Where(x => x.player.playerId != currentPlayer.player.playerId).FirstOrDefault();

			MessageProcessor.SendMessageToPlayer(currentPlayer.player, new Message() { Type = "Turn", Data = "true" });
			MessageProcessor.SendMessageToPlayer(nextPlayer.player, new Message() { Type = "Turn", Data = "false" });

			//TODO Send message to players that game started and his type
		}

		public Message Move(Player player, int x , int y) 
		{
			Message answer = new Message() { Type = "Error", Data = "Unknow error" };
			if (player.playerId == currentPlayer.player.playerId)
			{
				if (desk[y, x] == DeskCell.empty)
				{
					desk[y, x] = (DeskCell)Enum.Parse(typeof(DeskCell), currentPlayer.type.ToString());
					string deskString = JsonConvert.SerializeObject(desk); //DeskToString(desk);
					MessageProcessor.SendMessageToPlayer(nextPlayer.player, new Message() { Type = "Desk", Data = deskString });
					MessageProcessor.SendMessageToPlayer(currentPlayer.player, new Message() { Type = "Desk", Data = deskString });


					answer = new Message() { Type = "Moved", Data = $"{x},{y}" };
					CheckWin();

				}
			}
			else 
			{
				answer = new Message() { Type = "Error", Data = "Not your turn" };
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
						MessageProcessor.SendMessageToPlayer(player.player, new Message() { Type = "Win", Data = $"Score:{player.score}" });
					}
				}

				for (int x = 0; x < 3; x++)
				{
					if (new[] { desk[0, x], desk[1, x], desk[2, x] }.All(x => x == castedType))
					{
						player.score++;
						MessageProcessor.SendMessageToPlayer(player.player, new Message() { Type = "Win", Data = $"Score:{player.score}" });

					}
				}
				if (new[] { desk[0, 0], desk[1, 1], desk[2, 2] }.All(x => x == castedType))
				{
					player.score++;
					MessageProcessor.SendMessageToPlayer(player.player, new Message() { Type = "Win", Data = $"Score:{player.score}" });

				}
				if (new[] { desk[0, 2], desk[1, 1], desk[2, 0] }.All(x => x == castedType))
				{
					InGamePlayerList[type].score++;
					MessageProcessor.SendMessageToPlayer(player.player, new Message() { Type = "Win", Data = $"Score:{player.score}" });

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

			MessageProcessor.SendMessageToPlayer(currentPlayer.player, new Message() { Type = "Turn", Data = "True" });
			MessageProcessor.SendMessageToPlayer(nextPlayer.player, new Message() { Type = "Turn", Data = "False" });

		}

		class InGamePlayer 
		{
			public Player player;
			public PlayerType type;
			public int score;
		}
		enum DeskCell 
		{
			empty = 0,
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
