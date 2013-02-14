namespace augen.http
{
	public static class Ext
	{
		public static Http Http(this Project project, int port)
		{
			return new Http(project, port);
		}
	}
}