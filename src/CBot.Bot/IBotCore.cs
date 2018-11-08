using System.Threading.Tasks;

namespace CBot.Bot
{
	public interface IBotCore
	{
		Task Connect();

		Task Disconnect();
	}
}
