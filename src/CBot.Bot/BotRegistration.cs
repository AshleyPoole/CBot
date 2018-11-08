using System.Runtime.CompilerServices;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CBot.Bot
{
	public static class BotRegistration
	{
		public static IServiceCollection RegisterCBot(
			this IServiceCollection serviceCollection,
			IConfigurationSection configuration)
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

		public static IServiceCollection RegisterCBotAsHostedService(
			this IServiceCollection serviceCollection,
			IConfigurationSection configuration)
		{
			serviceCollection.RegisterCBot(configuration);
			serviceCollection.AddSingleton<IHostedService, CBotHostedService>();

			return serviceCollection;
		}
	}
}
