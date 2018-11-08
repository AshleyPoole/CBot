using Microsoft.Extensions.DependencyInjection;

namespace CBot.Bot.Domain
{
	public static class Startup
	{
		public static void RegisterComponents(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IDispatchIncomingMessageToMiddlewares, MessageDispatcher>();
		}
	}
}
