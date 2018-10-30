using System.Threading.Tasks;

using SlackConnector;

namespace SharpBotCore.Messaging.Slack
{
	public interface ISlackConnectionFactory
	{
		Task<ISlackConnection> GetConnection();
	}
}
