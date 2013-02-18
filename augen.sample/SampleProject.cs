using System.Collections.Generic;
using System.Net;

namespace augen.sample
{
	public class SampleProject : Project
	{
		public SampleProject() : base("Demo project", new Servers
		{
			{ "173.194.67.102", role => "google", role => "http", port => 80, path => "/calendar" },
			{ "google.com", role => "vip", role => "http", port => 80 },
			"microsoft.com",
			{"github.com", port => 26},
			{new [] {"bwin.com", "bwin.it"}, role => "http", role => "bwin", subpath => "en/p/about-us/responsible-gaming"}
		})
		{
			Http(123456)
			.Roles("vip", "google", "http")
			.Header("host", "google.com")
				.Get("/")
					.Test("content type", r => r.Content.Headers.ContentType)
					.Test("read response size > 0?", r => r.Content.ReadAsStringAsync().Result.Length > 0);

			Http()
			.Roles("bwin")
				.Get(o => "/" + o.subpath)
					.Success("http success?")
					.Test("status code with subpath", r => r.StatusCode)
					.Test("content length with subpath", r => r.Content.Headers.ContentLength);

			Http(80)
			.Roles("http")
			.Header("Accept-Encoding", "compress", "gzip")
				.Get("/")
					.Test("status code", r => r.StatusCode)
					.Test("content length", r => r.Content.Headers.ContentLength)
					.Test("content encoding", r => r.Content.Headers.ContentEncoding).End()
				.Get("robots.txt")
					.Test("robots!", r => r.StatusCode).End()
				.Get("humans.txt")
					.Test("humans!", r => r.StatusCode);

			Tcp(890)
				.IsOpen()
					.Test("is tcp port open (short form)?")
					.Test("is tcp port open?", b => b ? "yes it is" : "no it isn't");
		}

		protected override IEnumerable<Truthy> GetTruthies()
		{
			yield return Truthy<HttpStatusCode>(c => (int) c >= 200 && (int) c < 300);
			yield return Truthy<string>(_ => !_.Contains("no"));
			yield return Truthy<object>(_ => true);
		}
	}
}