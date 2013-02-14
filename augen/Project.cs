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
    }
}