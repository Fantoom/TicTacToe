using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TicTacToe_Server
{
	static class GameManager
	{
		public static void MakeMove(Player player, string move)
		{
			Coords coords = JsonConvert.DeserializeObject<Coords>(move);
			player.room.game.Move(player, coords.x, coords.y);
		}
		public static void SendMoveToPlayer(Player player, int x, int y)
		{
			Coords coords = new Coords() { x = x, y = y };
			string coordsJson = JsonConvert.SerializeObject(coords);
			Message message = new Message() { Type = "Move" , Data = coordsJson };
		}
	}

	public class Coords 
	{
		public int x;
		public int y;
	}
}
