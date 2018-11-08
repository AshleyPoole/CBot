using System.Threading.Tasks;

using SlackConnector;

namespace CBot.Messaging.Slack
{
	public interface ISlackConnectionFactory
	{
		Task<ISlackConnection> GetConnection();
	}
}
