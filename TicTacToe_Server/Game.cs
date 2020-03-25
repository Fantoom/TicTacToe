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
			InGamePlayerList.Add(InGameP1.type, InGameP1);
			InGamePlayerList.Add(InGameP2.type, InGameP2);

			currentPlayer = InGamePlayerList[(PlayerType)typeof(PlayerType).GetRandomEnumValue()];
			//TODO Send message to players that game started and his type
		}

		public void Move(Player player, int x , int y) 
		{
			var inGamePlayer = InGamePlayerList.Values.Where(x => x.player.playerId == player.playerId).FirstOrDefault();
			if (desk[y, x] == DeskCell.empty) 
			{
				desk[y, x] = (DeskCell)Enum.Parse(typeof(DeskCell), inGamePlayer.type.ToString());
				CheckWin();
			}
				
		}

		void CheckWin() 
		{
			foreach (PlayerType type in (PlayerType[])Enum.GetValues(typeof(PlayerType)))
			{
				var castedType = (DeskCell)Enum.Parse(typeof(DeskCell), type.ToString());
				for (int y = 0; y < 3; y++)
				{
					if (new[] { desk[y, 0], desk[y, 1], desk[y, 2] }.All(x => x == castedType))
					{
						InGamePlayerList[type].score++;
						//TODO notify for win
					}
				}

				for (int x = 0; x < 3; x++)
				{
					if (new[] { desk[0, x], desk[1, x], desk[2, x] }.All(x => x == castedType))
					{
						InGamePlayerList[type].score++;
						//TODO notify for win
					}
				}
				if (new[] { desk[0, 0], desk[1, 1], desk[2, 2] }.All(x => x == castedType))
				{
					InGamePlayerList[type].score++;
					//TODO notify for win
				}
				if (new[] { desk[0, 2], desk[1, 1], desk[2, 0] }.All(x => x == castedType))
				{
					InGamePlayerList[type].score++;
					//TODO notify for win
				}
			}
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
			X,
			O
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
