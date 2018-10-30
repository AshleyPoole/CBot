using System.Threading.Tasks;

using SharpBotCore.Bot.Domain;

using SlackConnector;

namespace SharpBotCore.Messaging.Slack
{
	public class SlackConnectionFactory : ISlackConnectionFactory
	{
		private readonly BotConfiguration configuration;

		public SlackConnectionFactory(BotConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public async Task<ISlackConnection> GetConnection()
		{
			var connector = new SlackConnector.SlackConnector();
			var connection = await connector.Connect(this.configuration.SlackApiKey);

			return connection;
		}
	}
}