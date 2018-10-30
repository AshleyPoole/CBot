namespace SharpBotCore.Middleware.Domain.Handlers
{
	public class AlwaysMatchHandler : IHandler
	{
		public bool CanHandle(string message)
		{
			return true;
		}

		public string HelpText => string.Empty;
	}
}
