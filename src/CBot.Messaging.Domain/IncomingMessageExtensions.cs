using System;
using System.Collections.Generic;
using System.Linq;

namespace CBot.Messaging.Domain
{
	public static class IncomingMessageExtensions
	{
		/// <summary>
		/// Will generate a message to be sent the channel the message arrived from
		/// </summary>
		public static ResponseMessage ReplyToChannel(this IncomingMessage incomingMessage, string text, Attachment attachment = null)
		{
			if (attachment == null)
				return ResponseMessage.ChannelMessage(incomingMessage.Channel, text, attachments: null);

			var attachments = new List<Attachment> { attachment };
			return incomingMessage.ReplyToChannel(text, attachments);
		}

		/// <summary>
		/// Will generate a message to be sent the channel the message arrived from
		/// </summary>
		public static ResponseMessage ReplyToChannel(this IncomingMessage incomingMessage, string text, List<Attachment> attachments)
		{
			return ResponseMessage.ChannelMessage(incomingMessage.Channel, text, attachments);
		}

		/// <summary>
		/// Will send a DirectMessage reply to the use who sent the message
		/// </summary>
		public static ResponseMessage ReplyDirectlyToUser(this IncomingMessage incomingMessage, string text)
		{
			return ResponseMessage.DirectUserMessage(incomingMessage.UserId, text);
		}

		/// <summary>
		/// Will display that the bot is typing.
		/// </summary>
		public static ResponseMessage IndicateTyping(this IncomingMessage incomingMessage)
		{
			switch (incomingMessage.ChannelType)
			{
				case ChannelType.Channel:
					return incomingMessage.IndicateTypingOnChannel();
				case ChannelType.DirectMessage:
					return incomingMessage.IndicateTypingOnDirectMessage();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Will display that the bot is typing on the current channel.
		/// </summary>
		public static ResponseMessage IndicateTypingOnChannel(this IncomingMessage incomingMessage)
		{
			return ResponseMessage.ChannelMessage(incomingMessage.Channel, string.Empty, attachments: null, message: new TypingIndicatorMessage());
		}

		/// <summary>
		/// Indicates on the DM channel that the bot is typing.
		/// </summary>
		public static ResponseMessage IndicateTypingOnDirectMessage(this IncomingMessage incomingMessage)
		{
			return ResponseMessage.DirectUserMessage(
				incomingMessage.UserChannel,
				incomingMessage.UserId,
				text: string.Empty,
				message: new TypingIndicatorMessage());
		}

		/// <summary>
		/// Returns the message text without the bots name.
		/// </summary>
		public static string GetMessageTextWithoutBotName(this IncomingMessage incomingMessage)
		{
			var messageText = incomingMessage.FullText ?? string.Empty;
			var isOnPrivateChannel = incomingMessage.ChannelType == ChannelType.DirectMessage;

			string[] myNames =
			{
				incomingMessage.BotName + ":",
				incomingMessage.BotName,
				$"<@{incomingMessage.BotId}>:",
				$"<@{incomingMessage.BotId}>",
				$"@{incomingMessage.BotName}:",
				$"@{incomingMessage.BotName}",
			};

			var handle = myNames.FirstOrDefault(x => messageText.StartsWith(x, StringComparison.OrdinalIgnoreCase));
			if (string.IsNullOrEmpty(handle) && !isOnPrivateChannel)
			{
				return string.Empty;
			}

			if (string.IsNullOrEmpty(handle) && isOnPrivateChannel)
			{
				return messageText;
			}

			return messageText.Substring(handle.Length).Trim();
		}
	}
}
