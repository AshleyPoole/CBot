using System.Collections.Generic;

using SharpBotCore.Messaging.Domain;
using SharpBotCore.Middleware.Domain.Handlers;

namespace SharpBotCore.Middleware.Domain.BuiltIn
{
	internal class HelloMiddleware : MiddlewareBase
	{
		public HelloMiddleware()
		{
			this.HandlerMappings = new[]
							{
								new HandlerMapping
								{
									Handlers = new IHandler[]
													{
														new StartsWithHandler("hello")
													},
									Description = "Bot returns hello",
									EvaluatorFunc = this.HelloHandler
								}
							};
		}

		private IEnumerable<ResponseMessage> HelloHandler(IncomingMessage message, IHandler matchedHandle)
		{
			yield return message.ReplyToChannel($"Hello @{message.Username}.");
		}
	}
}
