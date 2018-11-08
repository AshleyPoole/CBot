using System;
using System.Linq;

namespace CBot.Middleware.Domain.Handlers
{
	public class ExactMatchHandler : IHandler
	{
		private readonly string exactMatchText;

		public ExactMatchHandler(string messageToMatch)
		{
			this.exactMatchText = messageToMatch ?? string.Empty;
		}

		public bool CanHandle(string message)
		{
			return (message ?? string.Empty).Equals(this.exactMatchText, StringComparison.OrdinalIgnoreCase);
		}

		public string HelpText => this.exactMatchText;

		public static IHandler[] For(params string[] messagesToMatch)
		{
			return messagesToMatch
				.Select(x => new ExactMatchHandler(x))
				.Cast<IHandler>()
				.ToArray();
		}
	}
}
