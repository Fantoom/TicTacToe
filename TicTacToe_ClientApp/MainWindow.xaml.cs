using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TicTacToe_Client;

namespace TicTacToe_ClientApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Client client;

		
		public MainWindow()
		{
			InitializeComponent();
			client = App.instance.client;
		}

		private void CreateRoom_Click(object sender, RoutedEventArgs e)
		{

		}

		private void JoinByID_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
