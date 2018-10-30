using System.Collections.Generic;

namespace SharpBotCore.Messaging.Domain
{
	public class ResponseMessage
	{
		public string Text { get; set; }

		public string Channel { get; set; }

		public string UserId { get; set; }

		public ChannelType ChannelType { get; set; }

		public List<Attachment> Attachments { get; set; }

		public static ResponseMessage DirectUserMessage(string userId, string text, ResponseMessage message = null)
		{
			return DirectUserMessage(string.Empty, userId, text, message);
		}

		public static ResponseMessage DirectUserMessage(string userChannel, string userId, string text, ResponseMessage message = null)
		{
			if (message == null)
				message = new ResponseMessage();

			message.Channel = userChannel;
			message.ChannelType = ChannelType.DirectMessage;
			message.UserId = userId;
			message.Text = text;

			return message;
		}

		public static ResponseMessage ChannelMessage(string channel, string text, Attachment attachment, ResponseMessage message = null)
		{
			List<Attachment> attachments = null;
			if (attachment != null)
				attachments = new List<Attachment> { attachment };

			return ChannelMessage(channel, text, attachments, message);
		}

		public static ResponseMessage ChannelMessage(string channel, string text, List<Attachment> attachments, ResponseMessage message = null)
		{
			if (message == null)
				message = new ResponseMessage();

			message.Channel = channel;
			message.ChannelType = ChannelType.Channel;
			message.Text = text;
			message.Attachments = attachments;

			return message;
		}
	}
}
