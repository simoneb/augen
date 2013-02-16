using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace augen.Http
{
	public class Http : Connection<Http, HttpClient>
	{
		private readonly IDictionary<string, string[]> _headers = new Dictionary<string, string[]>();

		public Http(Project project, int port) : base(project, _port => port)
		{
		}

		public Get Get(string path)
		{
			return new Get(this, path);
		}

		protected override HttpClient Open(string serverName, dynamic options)
		{
			var client = new HttpClient {BaseAddress = new UriBuilder("http", serverName).Uri};

			foreach (var header in _headers)
				client.DefaultRequestHeaders.Add(header.Key, header.Value);

			return client;
		}

		protected override void Close(HttpClient connection)
		{
			connection.Dispose();
		}

		public Http Header(string name, string value, params string[] values)
		{
			_headers[name] = new[]{value}.Concat(values).ToArray();
			return this;
		}
	}
}