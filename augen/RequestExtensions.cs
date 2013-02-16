namespace augen
{
	public static class RequestExtensions
	{
		public static TSelf Test<TParent, TSelf, TConnectio>(this Request<TParent, TSelf, TConnectio, bool> request,
		                                                     string description) where TParent : Connection<TParent, TConnectio> where TSelf : Request<TParent, TSelf, TConnectio, bool>
		{
			request.Add(new Test(description, b => b));
			return (TSelf) request;
		}
	}
}