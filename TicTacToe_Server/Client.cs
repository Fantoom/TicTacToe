using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TicTacToe_Server
{
	class Client
	{
		public Guid id { get; }
		public Socket socket { get; }
		public Client(Guid id, Socket socket)
		{
			this.id = id;
			this.socket = socket;
		}
	}
}
