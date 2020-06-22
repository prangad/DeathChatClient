using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeathChat
{
	class Program
	{
		private static Socket _clientSocket = new Socket
			(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Title = "DeathChat";
			LoopConnect();

			Thread receiveThread = new Thread(new ThreadStart(ReceiveLoop));
			Thread sendThread = new Thread(new ThreadStart(SendLoop));
			receiveThread.Start();
			sendThread.Start();
		}

		private static void ReceiveLoop()
		{
			while (true)
			{
				byte[] receivedBuf = new byte[1024];
				int rec = _clientSocket.Receive(receivedBuf);
				byte[] data = new byte[rec];
				Array.Copy(receivedBuf, data, rec);
				Console.WriteLine(Encoding.ASCII.GetString(data));
			}
		}

		private static void SendLoop()
		{
			while (true)
			{
				string msg = Console.ReadLine();
				byte[] buffer = Encoding.ASCII.GetBytes(msg);
				_clientSocket.Send(buffer);
			}
		}

		private static void LoopConnect()
		{
			int attempts = 0;

			while (!_clientSocket.Connected)
			{
				try
				{
					attempts++;
					_clientSocket.Connect(IPAddress.Loopback, 100);
				}
				catch (SocketException)
				{
					Console.Clear();
					Console.WriteLine("[DEBUG] Connection Attempts: " + attempts.ToString());
				}
			}
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("[DeathChat] Connected...");
		}
	}
}
