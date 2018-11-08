using System;
using System.Linq;

namespace CBot.Middleware.Domain.Handlers
{
	public class StartsWithHandler : IHandler
	{
		private readonly string messageStartsWith;

		public StartsWithHandler(string messageStartsWith)
		{
			this.messageStartsWith = messageStartsWith ?? string.Empty;
		}

		public bool CanHandle(string message)
		{
			return (message ?? string.Empty).StartsWith(this.messageStartsWith, StringComparison.OrdinalIgnoreCase);
		}

		public string HelpText => this.messageStartsWith;

		public static IHandler[] For(params string[] messagesStartsWith)
		{
			return messagesStartsWith
				.Select(x => new StartsWithHandler(x))
				.Cast<IHandler>()
				.ToArray();
		}
	}
}
