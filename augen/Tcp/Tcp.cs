using System;
using System.Net.Sockets;

namespace augen.Tcp
{
	public class Tcp : Connection<Tcp, Socket>
	{
		public Tcp(Project project, int port, bool connectThrows) : base(project, _port => port, _connectThrows => connectThrows)
		{
		}

		public TcpIsOpen IsOpen()
		{
			return new TcpIsOpen(this);
		}

		protected override Socket Open(string serverName, dynamic options)
		{
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			
			try
			{
				socket.Connect(serverName, options.port);
			}
			catch
			{
				if (options.connectThrows)
					throw;
			}
			
			return socket;
		}

		protected override void Close(Socket connection)
		{
			connection.Close();
		}
	}
}