using System.Collections.Generic;

using CBot.Messaging.Domain;

namespace CBot.Bot.Domain
{
	public interface IDispatchIncomingMessageToMiddlewares
	{
		IEnumerable<ResponseMessage> Dispatch(IncomingMessage message);
	}
}
