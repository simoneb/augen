using System;
using System.Net;

namespace augen.http
{
	public class Get : Request<HttpWebRequest, HttpWebResponse>
	{
		public Get(Connection connection, string path) : base(connection, Tuple.Create("path", (object)path))
		{
		}

		protected override HttpWebResponse Execute(HttpWebRequest request, Options options)
		{
			request.Method = "GET";

			return (HttpWebResponse) request.GetResponse();
		}

		protected override void Close(HttpWebResponse response)
		{
			response.Close();
		}
	}
}