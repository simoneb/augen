using System;
using System.Collections.Generic;

namespace augen
{
    public abstract class Project
    {
	    public string Name { get; private set; }
        public Servers Servers { get; private set; }
        public List<Connection> Connections { get; private set; }

        protected Project(string name, Servers servers)
        {
            Name = name;
            Servers = servers;
            Connections = new List<Connection>();
        }

        internal void Add(Connection connection)
        {
            Connections.Add(connection);
        }

		protected Http.Http Http(int port = 80)
		{
			return new Http.Http(this, port);
		}

		protected Tcp.Tcp Tcp(int port, int connectTimeout = 1000, bool connectThrows = false)
		{
			return new Tcp.Tcp(this, port, connectTimeout, connectThrows);
		}

	    protected internal virtual IEnumerable<Truthy> GetTruthies()
	    {
		    yield break;
	    }

	    protected static Truthy Truthy<T>(Func<T, bool> condition)
		{
			return new Truthy(typeof(T), o => condition((T) o));
		}
    }
}