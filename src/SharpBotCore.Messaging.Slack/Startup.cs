using Microsoft.Extensions.DependencyInjection;

namespace SharpBotCore.Messaging.Slack
{
	public static class Startup
	{
		public static void RegisterComponents(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<ISlackConnectionFactory, SlackConnectionFactory>();
			serviceCollection.AddTransient<ISlackHelper, SlackHelper>();
		}
	}
}
