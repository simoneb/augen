namespace augen
{
	public abstract class NonTerminal<T> : NonTerminal where T : NonTerminal<T>
	{
		protected Project Project { get; private set; }

		protected NonTerminal(Project project)
		{
			Project = project;
		}
	}

	public abstract class NonTerminal{}
}