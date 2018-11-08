using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CBot.Bot.Domain;
using CBot.Messaging.Domain;
using CBot.Messaging.Slack;

using SlackConnector;
using SlackConnector.Models;

namespace CBot.Bot
{
	public class BotCore : IBotCore
	{
		private readonly ISlackConnectionFactory connectionFactory;

		private readonly IDispatchIncomingMessageToMiddlewares messageDispatcher;

		private readonly ISlackHelper slackHelper;

		private readonly ILogger<BotCore> log;

		private ISlackConnection connection;

		public BotCore(IDispatchIncomingMessageToMiddlewares messageDispatcher, ISlackConnectionFactory connectionFactory, ISlackHelper slackHelper, ILogger<BotCore> log)
		{
			this.connectionFactory = connectionFactory;
			this.messageDispatcher = messageDispatcher;
			this.slackHelper = slackHelper;
			this.log = log;
		}

		public async Task Connect()
		{
			this.connection = await this.connectionFactory.GetConnection();

			this.connection.OnMessageReceived += this.MessageReceived;
			this.connection.OnDisconnect += this.OnDisconnect;
			this.connection.OnReconnecting += this.OnReconnecting;
			this.connection.OnReconnect += this.OnReconnect;

			this.log.LogInformation($"Connected to {this.connection.Team.Name} as @{this.connection.Self.Name}!");
		}

		public Task Disconnect()
		{
			return this.connection.IsConnected ? this.connection.Close() : Task.CompletedTask;
		}

		private async Task MessageReceived(IncomingMessage incomingMessage)
		{
			try
			{
				foreach (var responseMessage in this.messageDispatcher.Dispatch(incomingMessage))
				{
					await this.SendMessage(responseMessage);
				}
			}
			catch (Exception ex)
			{
				this.log.LogError($"Error while processing message ({incomingMessage.TraceId}) from user {incomingMessage.Username}. Exception: {ex}");
			}
		}

		private async Task SendMessage(ResponseMessage responseMessage)
		{
			var chatHub = await this.slackHelper.GetChatHub(responseMessage, this.connection);

			if (chatHub != null)
			{
				if (responseMessage is TypingIndicatorMessage)
				{
					this.log.LogDebug($"Indicating typing on channel '{chatHub.Name}'");
					await this.connection.IndicateTyping(chatHub);
				}
				else
				{
					var botMessage = new BotMessage
									{
										ChatHub = chatHub,
										Text = responseMessage.Text,
										Attachments = this.slackHelper.ConvertAttachmentsToSlackAttachments(responseMessage.Attachments)
									};

					var textTrimmed = botMessage.Text.Length > 50 ? botMessage.Text.Substring(0, 50) + "..." : botMessage.Text;
					this.log.LogDebug($"Sending message '{textTrimmed}'");

					await this.connection.Say(botMessage);
				}
			}
			else
			{
				this.log.LogError($"Unable to find channel for message '{responseMessage.Text}'. Message not sent");
			}
		}

		private void OnDisconnect()
		{
			this.log.LogInformation("Disconnected from server, attempting to reconnect...");
			this.Reconnect();
		}

		private Task OnReconnect()
		{
			this.log.LogInformation("Connection restored!");
			return Task.CompletedTask;
		}

		private Task OnReconnecting()
		{
			this.log.LogInformation("Attempting to reconnect to Slack...");
			return Task.CompletedTask;
		}

		private async Task MessageReceived(SlackMessage message)
		{
			var stopwatch = Stopwatch.StartNew();
			var traceId = Guid.NewGuid();

			this.log.LogDebug($"[Message processing started ({traceId})- User:'{message.User.Name}']");

			var incomingMessage = await this.slackHelper.ConvertMessageToIncomingMessageType(message, traceId, this.connection);
			await this.MessageReceived(incomingMessage);

			stopwatch.Stop();

			this.log.LogInformation($"[Message processing finished ({traceId}) - Took {stopwatch.ElapsedMilliseconds} milliseconds]");
		}

		private void Reconnect()
		{
			this.log.LogInformation("Reconnecting...");
			if (this.connection != null)
			{
				this.connection.OnMessageReceived -= this.MessageReceived;
				this.connection.OnDisconnect -= this.OnDisconnect;
				this.connection = null;
			}

			this.Connect()
				.ContinueWith(task =>
				{
					if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
					{
						this.log.LogInformation("Connection restored.");
					}
					else
					{
						this.log.LogError($"Error while reconnecting. Ex: {task.Exception}");
					}
				});
		}
	}
}