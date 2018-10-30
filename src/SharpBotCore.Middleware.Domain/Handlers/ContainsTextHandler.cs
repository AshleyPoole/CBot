using System;
using System.Linq;

namespace SharpBotCore.Middleware.Domain.Handlers
{
	public class ContainsTextHandler : IHandler
	{
		private readonly string containsText;

		public ContainsTextHandler(string containsText)
		{
			this.containsText = containsText ?? string.Empty;
		}

		public bool CanHandle(string message)
		{
			return (message ?? string.Empty).IndexOf(this.containsText, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		public string HelpText => this.containsText;

		public static IHandler[] For(params string[] containsText)
		{
			return containsText
				.Select(x => new ContainsTextHandler(x))
				.Cast<IHandler>()
				.ToArray();
		}
	}
}
