using System.Collections.Generic;

namespace CBot.Middleware.Domain
{
	public abstract class MiddlewareBase : IMiddleware
	{
		protected MiddlewareBase()
		{
			this.HandlerMappings = new HandlerMapping[0];
		}

		public IEnumerable<HandlerMapping> HandlerMappings { get; set; }
	}
}
