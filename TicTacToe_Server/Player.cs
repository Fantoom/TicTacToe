using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using NetCoreServer;
namespace TicTacToe_Server
{
	class Player
	{
		public Guid PlayerId { get; private set; }
		public TcpSession Session { get; private set; }
		public Room Room { get; set; }

		public Player(TcpSession session)
		{
			PlayerId = session.Id;
			Session = session;
		}
	}
}
