namespace CBot.Middleware.Domain.Handlers
{
	public interface IHandler
	{
		bool CanHandle(string message);

		string HelpText { get; }
	}
}
