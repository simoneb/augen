using System;
using System.Net.Http;
using System.Linq;

namespace augen.Http
{
	public class Get : Request<Http, Get, HttpClient, HttpResponseMessage>
	{
		public Get(Http http, string path) : base(http, _path => path)
		{
		}

		public Get(Http http, Func<object, string> path) : base(http, _path => path)
		{
		}

		public Get Success(string description)
		{
			return Test(description, r => r.IsSuccessStatusCode);
		}

		protected override HttpResponseMessage Execute(HttpClient connection, dynamic options)
		{
			return connection.GetAsync((string) options.path).Result;
		}

		protected override void Close(HttpResponseMessage response)
		{
			response.Dispose();
		}
	}
}