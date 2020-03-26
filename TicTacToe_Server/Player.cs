using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using NetCoreServer;
namespace TicTacToe_Server
{
	class Player
	{
		public Guid playerId;
		public TcpSession session;
		public Room room;

		public Player(TcpSession session)
		{
			this.playerId = session.Id;
			this.session = session;
		}
	}
}
