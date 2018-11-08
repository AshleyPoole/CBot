using Microsoft.Extensions.DependencyInjection;

using CBot.Middleware.Domain.BuiltIn;

namespace CBot.Middleware.Domain
{
	public static class Startup
	{
		public static void RegisterComponents(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<IMiddleware, HelloMiddleware>();
		}
	}
}
