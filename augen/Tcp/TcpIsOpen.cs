using System.Net.Sockets;

namespace augen.Tcp
{
	public class TcpIsOpen : Request<Tcp, TcpIsOpen, Socket, bool>
	{
		public TcpIsOpen(Tcp tcp) : base(tcp)
		{
		}

		protected override void Close(bool response)
		{

		}

		protected override bool Execute(Socket connection, dynamic options)
		{
			return connection.Connected;
		}
	}
}