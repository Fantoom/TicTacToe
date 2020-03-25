using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace TicTacToe_Server
{
	class Player
	{
		public Guid playerId;
		public Client client;
		public Room room;

		public Player(Client client)
		{
			this.playerId = client.id;
			this.client = client;
		}
	}
}
