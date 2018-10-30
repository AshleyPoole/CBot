# SharpBot (Aka SharpBotCore)

SharpBot is a SlackBot for C#, originially forked from [Noobot](https://github.com/noobot/noobot).

## About

As mentioned above, SharpBot is originially forked from [Noobot](https://github.com/noobot/noobot) but has a different take on dependency injection and logging. Mainly, the DI container is external from the core bots code, so consumers can more easily extend the framework, as well as logging now uses the `ILogger` interface. Both of these changes allow for improved consumption in .NET Core.

## Features

 - Available as a Nuget package to integrate into your apps (See [examples](https://github.com/noobot/noobot.examples))
 - DI support out of the box
 - Automatically builds up `help` text with all supported commands
 - Middleware can send multiple messages for each message received
 - Supports long running processes (async)
 - Typing Indicator - indicate to the end user that the bot has received the message and is processing the request

## Installation / Usage

```
Install-Package SharpBotCore
```

Please note that you will need to create a config.json file with your bot's api key. This should live under:
`src/Noobot.Runner/Configuration`

## Examples

You can find some examples of how to use the Nuget package in different scenarios at [Noobot.Examples](https://github.com/noobot/Noobot.Examples)

## How to customise

To customise Noobot please have a look at our [wiki: https://github.com/noobot/noobot/wiki](https://github.com/noobot/noobot/wiki)