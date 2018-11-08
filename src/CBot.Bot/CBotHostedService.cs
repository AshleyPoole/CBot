using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace CBot.Bot
{
	public class CBotHostedService : IHostedService
	{
		private readonly IBotCore botCore;

		public CBotHostedService(IBotCore botCore)
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
