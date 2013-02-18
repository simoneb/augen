using System;
using System.Net.Sockets;

namespace augen.Tcp
{
	public class Tcp : Connection<Tcp, Socket>
	{
		private const int ConnectionTimedOut = 10060;

		public Tcp(Project project, int port, int connectTimeout, bool connectThrows)
			: base(project, _port => port, _connectTimeout => connectTimeout, _connectThrows => connectThrows)
		{
		}

		public TcpIsOpen IsOpen()
		{
			return new TcpIsOpen(this);
		}

		protected override Socket Open(string serverName, dynamic options)
		{
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			var result = (IAsyncResult)socket.BeginConnect(serverName, options.port, null, null);
			result.AsyncWaitHandle.WaitOne(options.connectTimeout);

			if(socket.Connected)
				try
				{
					socket.EndConnect(result);
				}
				catch
				{
					if (options.connectThrows)
						throw;
				}
			else if(options.connectThrows)
				throw new SocketException(ConnectionTimedOut);
			
			return socket;
		}

		protected override void Close(Socket connection)
		{
			connection.Close();
		}
	}
}