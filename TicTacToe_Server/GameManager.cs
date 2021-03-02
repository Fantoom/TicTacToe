using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TicTacToe_Server
{
	static class GameManager
	{
		public static Message MakeMove(Player player, string move)
		{
			try
			{
				return player.Room.Game.Move(player, int.Parse(move.Split(',')[0]), int.Parse(move.Split(',')[1]));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);

				return new Message() { Type = MessageType.Error.ToString(), Data = e.Message };
			}
		
		}
		public static void SendMoveToPlayer(Player player, int x, int y)
		{
			Message message = new Message() { Type = MessageType.Move.ToString(), Data = $"{x},{y}"};
		}
	}
}
