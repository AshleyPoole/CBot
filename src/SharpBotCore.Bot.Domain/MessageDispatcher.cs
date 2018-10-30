using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpBotCore.Messaging.Domain;
using SharpBotCore.Middleware.Domain;

namespace SharpBotCore.Bot.Domain
{
	public class MessageDispatcher : IDispatchIncomingMessageToMiddlewares
	{
		private readonly IEnumerable<IMiddleware> middlewares;

		private readonly List<HandlerMapping> allHandlerMappings;

		public MessageDispatcher(IEnumerable<IMiddleware> middlewares)
		{
			this.middlewares = middlewares;
			this.allHandlerMappings = this.middlewares.SelectMany(x => x.HandlerMappings).ToList();
		}

		public IEnumerable<ResponseMessage> Dispatch(IncomingMessage message)
		{
			if (ShouldReturnHelpResponseInstead(message))
			{
				yield return message.ReplyToChannel(this.GetHelpTextForMiddlewares(message.BotName));
				yield break;
			}

			foreach (var middleware in this.middlewares)
			{
				foreach (var handlerMapping in middleware.HandlerMappings)
				{
					foreach (var handler in handlerMapping.Handlers)
					{
						var messageText = handlerMapping.MessageShouldTargetBot ? message.TargetedText : message.FullText;

						if (handler.CanHandle(messageText))
						{
							foreach (var responseMessage in handlerMapping.EvaluatorFunc(message, handler))
							{
								yield return responseMessage;
							}

							if (!handlerMapping.ShouldContinueProcessing)
							{
								yield break;
							}
						}
					}
				}
			}
		}

		private static bool ShouldReturnHelpResponseInstead(IncomingMessage message) =>
			message.TargetedText.StartsWith("help") || message.TargetedText.StartsWith("man")
													|| message.TargetedText.StartsWith("What can you do");

		private string GetHelpTextForMiddlewares(string botName)
		{
			var builder = new StringBuilder();
			builder.Append(">>>");

			var supportedCommands = this.GetSupportedCommands();
			foreach (var supportedCommand in supportedCommands)
			{
				var description = supportedCommand.Description.Replace("@{bot}", $"@{botName}");

				description = description.Trim();

				if (!description.EndsWith("."))
				{
					description = description + ".";
				}

				var commandExample = supportedCommand.Example;
				if (!string.IsNullOrWhiteSpace(commandExample))
				{
					commandExample = $" I.e ({commandExample.Trim()})";
				}

				builder.AppendFormat("{0} - {1}{2}\n", supportedCommand.Command.Trim(), description, commandExample);
			}

			return builder.ToString();
		}

		private IEnumerable<CommandDescription> GetSupportedCommands()
		{
			foreach (var handlerMapping in this.allHandlerMappings)
			{
				if (!handlerMapping.VisibleInHelp)
				{
					continue;
				}

				yield return new CommandDescription
							{
								Command = string.Join(" | ", handlerMapping.Handlers.Select(x => $"`{x.HelpText}`").OrderBy(x => x)),
								Description = handlerMapping.Description
							};
			}
		}
	}
}