namespace augen.console
{
	public class DemoProject : Project
	{
		public DemoProject() : base("Demo project", new Servers
		{
			{ "173.194.67.102", role => "google", role => "http", port => 80, path => "/calendar" },
			{ "google.com", role => "vip", role => "http", port => 80 },
			"microsoft.com",
			{"github.com", port => 26},
			{new Names {"bwin.com", "bwin.it"}, role => "http"}
		})
		{
			Http(123456)
			.Roles("vip", "google", "http")
			.Header("host", "google.com")
				.Get("/")
					.Test("content type", response => response.Content.Headers.ContentType);

			Http(80)
			.Roles("http")
			.Header("Accept-Encoding", "compress", "gzip")
				.Get("/")
					.Test("status code", response => response.StatusCode)
					.Test("content length", response => response.Content.Headers.ContentLength)
					.Test("content encoding", response => response.Content.Headers.ContentEncoding).End()
				.Get("robots.txt")
					.Test("robots!", response => response.StatusCode).End()
				.Get("humans.txt")
					.Test("humans!", response => response.StatusCode);

			Tcp(80)
				.IsOpen()
					.Test("is tcp port open (short form)?")
					.Test("is tcp port open?", b => b ? "yes it is" : "no it isn't");
		}
	}
}