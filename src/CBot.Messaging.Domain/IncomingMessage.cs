using System;

namespace CBot.Messaging.Domain
{
	public class IncomingMessage
	{
		/// <summary>
		/// The Slack UserId of whoever sent the message
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Username of whoever sent the message
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// The channel used to send a DirectMessage back to the user who sent the message. 
		/// Note: this might be empty if the Bot hasn't talked to them privately before, but the bot will join the DM automatically if required.
		/// </summary>
		public string UserChannel { get; set; }

		/// <summary>
		/// The email of the user that sent the message
		/// </summary>
		public string UserEmail { get; set; }

		/// <summary>
		/// Contains the untainted raw Text that comes in from Slack. This hasn't been URL decoded.
		/// </summary>
		public string RawText { get; set; }

		/// <summary>
		/// Contains the URL decoded text from the message.
		/// </summary>
		public string FullText { get; set; }

		/// <summary>
		/// Contains the text minus any Bot targeting text (e.g. "@bot {blah}" turns into "{blah}")
		/// </summary>
		public string TargetedText => this.GetMessageTextWithoutBotName();

		/// <summary>
		/// The 'channel' the message occured on. This might be a DirectMessage channel.
		/// </summary>
		public string Channel { get; set; }

		/// <summary>
		/// The type of channel the message arrived on
		/// </summary>
		public ChannelType ChannelType { get; set; }

		/// <summary>
		/// Detects if the Bots name is mentioned anywhere in the text
		/// </summary>
		public bool BotIsMentioned { get; set; }

		/// <summary>
		/// The Bots name - configured in the chat client
		/// </summary>
		public string BotName { get; set; }

		/// <summary>
		/// The Bots UserId
		/// </summary>
		public string BotId { get; set; }

		/// <summary>
		/// Random Guid assigned for logging so actions resulting from a single message can be traced. This should be included in all logging statements.
		/// </summary>
		public Guid TraceId { get; set; }
	}
}