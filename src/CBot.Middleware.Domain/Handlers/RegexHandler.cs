using System.Linq;
using System.Text.RegularExpressions;

namespace CBot.Middleware.Domain.Handlers
{
	public class RegexHandler : IHandler
	{
		private readonly Regex regex;

		public RegexHandler(string regexPattern, RegexOptions regexOptions = RegexOptions.IgnoreCase)
			: this(regexPattern, regexPattern, regexOptions)
		{ }

		public RegexHandler(string regexPattern, string helpText, RegexOptions regexOptions = RegexOptions.IgnoreCase)
		{
			this.regex = new Regex(regexPattern ?? string.Empty, regexOptions);
			this.HelpText = helpText ?? string.Empty;
		}

		public bool CanHandle(string message)
		{
			return this.regex.IsMatch(message ?? string.Empty);
		}

		public string HelpText { get; }

		public static IHandler[] For(params string[] regexPatterns)
		{
			return regexPatterns
				.Select(x => new RegexHandler(x))
				.Cast<IHandler>()
				.ToArray();
		}
	}
}
