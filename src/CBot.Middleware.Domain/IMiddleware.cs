using System.Collections.Generic;

namespace CBot.Middleware.Domain
{
	public interface IMiddleware
	{
		IEnumerable<HandlerMapping> HandlerMappings { get; }
	}
}
