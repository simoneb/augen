using System.Collections.Generic;

namespace augen
{
    public abstract class Project
    {
	    private readonly string _name;
	    private readonly Servers _servers;
	    private readonly List<TestDescriptor> _tests = new List<TestDescriptor>();

	    protected Project(string name, Servers servers)
	    {
		    _name = name;
		    _servers = servers;
	    }

	    internal void Add(TestDescriptor testDescriptor)
	    {
		    _tests.Add(testDescriptor);
	    }

	    internal void AcceptRunner(AbstractRunner runner)
	    {
		    runner.RunImpl(_name, _servers, _tests.ToArray());
	    }
    }
}