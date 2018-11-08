using System;
using System.Collections.Generic;

using CBot.Messaging.Domain;
using CBot.Middleware.Domain.Handlers;

namespace CBot.Middleware.Domain
{
	public class HandlerMapping
	{
		/// <summary>
		/// Handles to match on for incoming text, e.g. match exactly on "Hello"
		/// </summary>
		public IHandler[] Handlers { get; set; } = new IHandler[0];

		/// <summary>
		/// Description of what this handle does. This appears in the "help" function.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// The function that is evaluated on a matched handle
		/// </summary>
		public Func<IncomingMessage, IHandler, IEnumerable<ResponseMessage>> EvaluatorFunc { get; set; }

		/// <summary>
		/// Defaults to "False". If set to True then the pipeline isn't interrupted if a match occurs here. An example use case would be for logging.
		/// </summary>
		public bool ShouldContinueProcessing { get; set; }

		/// <summary>
		/// Defaults to "True". If set to false then any message is considered, even if it isn't targeted at the bot. e.g. @bot or in a private channel
		/// </summary>
		public bool MessageShouldTargetBot { get; set; } = true;

		/// <summary>
		/// Defaults to "True". Set to false to hide these commands in the help command.
		/// </summary>
		public bool VisibleInHelp { get; set; } = true;
	}
}
