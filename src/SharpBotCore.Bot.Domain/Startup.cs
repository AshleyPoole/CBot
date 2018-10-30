using Microsoft.Extensions.DependencyInjection;

namespace SharpBotCore.Bot.Domain
{
	public static class Startup
	{
		public static void RegisterComponents(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IDispatchIncomingMessageToMiddlewares, MessageDispatcher>();
		}
	}
}
