﻿using System;

namespace TicTacToe_Server
{
	class Program
	{
		static void Main(string[] args)
		{
			Server server = new Server();
			server.Start();
		}
	}
}