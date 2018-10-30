# SharpBotCore, aka SharpBot

SharpBotCore is a SlackBot for C#, originially forked from [Noobot](https://github.com/noobot/noobot).

## About

As mentioned above, SharpBotCore is originially forked from [Noobot](https://github.com/noobot/noobot) but has a different take on dependency injection (DI), logging and configuration.

Mainly, the DI container is external from the core bots code, so consumers can more easily extend the framework as they can add their own DI implementation of choice. Logging now uses the `ILogger` interface for easily of implemenation for consumers, and configuration can be consumed more easily.

I believe these changes allow for improved consumption in .NET Core projects. Currently targetting .NET Core 2.1+.

## Features

- Available as a Nuget package to integrate into your apps (see Installation / Usage below)
- DI support out of the box
- Automatically builds up `help` text with all supported commands
- Middleware can send multiple messages for each message received
- Supports long running processes
- Typing Indicator - indicate to the end user that the bot has received the message and is processing the request

## Installation / Usage

Below are the intructions to add SharpBotCore framework to your ASP.NET Core 2.1 project.

In your web project, add the NuGet package:

```plain
Install-Package SharpBotCore
```

To run SharpBotCore in the background, you'll need to register it as a hosted service. Create a file called `BotHost.cs` and add the following, replacing `SreBot.Web` with your own namespace:

```csharp
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using SharpBotCore.Bot;

namespace SreBot.Web
{
    public class BotHost : IHostedService
    {
        private readonly IBotCore botCore;

        public BotHost(IBotCore botCore)
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
```

In your `Startup.cs`, under the services registration section, register SharpBotCore:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...

    services.RegisterSharpBot(this.Configuration.GetSection("Bot"));

    services.AddSingleton<IHostedService, BotHost>();
}
```

In your application settings file, you will need to create a bot section and populate your Slack API key.

```plain
{
    "Bot": {
        "SlackApiKey": "YOUR_KEY"
    }
}
```

## Example Project

You can find an example project of how to use the Nuget package in ASP.NET Core 2.1 application as shown in this [SreBot.Web project](https://github.com/AshleyPoole/SreBot)

## Writing your own customised modules

TBD

## Inprogress / Outstanding

- Unit tests
- Improved documentation
- More examples