﻿using System.Net.Http;

namespace augen.Http
{
	public class Get : Request<Http, Get, HttpClient, HttpResponseMessage>
	{
		public Get(Http http, string path) : base(http, _path => path)
		{
		}

		protected override HttpResponseMessage Execute(HttpClient connection, dynamic options)
		{
			return connection.GetAsync((string)options.path).Result;
		}

		protected override void Close(HttpResponseMessage response)
		{
			response.Dispose();
		}
	}
}