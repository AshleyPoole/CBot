using System.Collections.Generic;

namespace SharpBotCore.Middleware.Domain
{
	public interface IMiddleware
	{
		IEnumerable<HandlerMapping> HandlerMappings { get; }
	}
}
