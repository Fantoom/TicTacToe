using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe_Client
{
	public class Message
	{
		public string Type { get; set; }
		public string Data { get; set; }

		public Message()
		{

		}

		public Message(string type, string data)
		{
			Type = type;
			Data = data;
		}
	}
}
