using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharpBotCore.Bot
{
	public static class BotRegistration
	{
		public static IServiceCollection RegisterSharpBot(this IServiceCollection serviceCollection, IConfigurationSection configuration)
		{
			Middleware.Domain.Startup.RegisterComponents(serviceCollection);
			Messaging.Slack.Startup.RegisterComponents(serviceCollection);
			Domain.Startup.RegisterComponents(serviceCollection);

			var botConfig = new Domain.BotConfiguration();
			configuration.Bind(botConfig);

			serviceCollection.AddSingleton(botConfig);
			serviceCollection.AddSingleton<IBotCore, BotCore>();

			return serviceCollection;
		}
	}
}
