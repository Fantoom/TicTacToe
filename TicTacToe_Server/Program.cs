using System;
using System.Threading;
namespace TicTacToe_Server
{
	class Program
	{
		static void Main(string[] args)
		{
			
			Server server = new Server();

			new Thread(() =>
			{
				//Thread.CurrentThread.IsBackground = true;
				/* run your code here */
				server.Start();
			}).Start();

			Console.WriteLine("not blocked");
		}
	}
}
