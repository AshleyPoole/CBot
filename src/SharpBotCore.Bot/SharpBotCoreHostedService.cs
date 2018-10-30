using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace SharpBotCore.Bot
{
	public class SharpBotCoreHostedService : IHostedService
	{
		private readonly IBotCore botCore;

		public SharpBotCoreHostedService(IBotCore botCore)
		{
			this.botCore = botCore;
		}

		public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			await this.botCore.Connect();
		}

		public async Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			await this.botCore.Disconnect();
		}
	}
}
