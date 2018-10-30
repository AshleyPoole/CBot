using System.Threading.Tasks;

namespace SharpBotCore.Bot
{
	public interface IBotCore
	{
		Task Connect();

		Task Disconnect();
	}
}
