using System.Collections.Generic;

using SharpBotCore.Messaging.Domain;

namespace SharpBotCore.Bot.Domain
{
	public interface IDispatchIncomingMessageToMiddlewares
	{
		IEnumerable<ResponseMessage> Dispatch(IncomingMessage message);
	}
}
