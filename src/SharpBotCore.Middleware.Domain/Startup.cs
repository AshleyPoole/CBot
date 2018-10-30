using Microsoft.Extensions.DependencyInjection;

using SharpBotCore.Middleware.Domain.BuiltIn;

namespace SharpBotCore.Middleware.Domain
{
	public static class Startup
	{
		public static void RegisterComponents(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<IMiddleware, HelloMiddleware>();
		}
	}
}
