using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TicTacToe_Client;
namespace TicTacToe_ClientApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        public Client client;
        string ip = "127.0.0.1"; // for future: load it from file
        int port = 11000;
        public static App instance;
        void App_Startup(object sender, StartupEventArgs e)
        {
            instance = this;
            //Console.WriteLine("Startup*********");
            client = new Client(ip, port);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
