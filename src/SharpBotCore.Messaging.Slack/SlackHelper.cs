using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SharpBotCore.Messaging.Domain;

using SlackConnector;
using SlackConnector.Models;

namespace SharpBotCore.Messaging.Slack
{
	public class SlackHelper : ISlackHelper
	{
		public SlackHelper()
		{
		}

		public async Task<IncomingMessage> ConvertMessageToIncomingMessageType(SlackMessage slackMessage, Guid traceId, ISlackConnection connection)
		{
			var incomingMessage = new IncomingMessage
								{
									RawText = slackMessage.Text,
									FullText = slackMessage.Text,
									UserId = slackMessage.User.Id,
									Username = this.GetUsername(slackMessage.User, connection),
									UserEmail = slackMessage.User.Email,
									Channel = slackMessage.ChatHub.Id,
									ChannelType = slackMessage.ChatHub.Type == SlackChatHubType.DM ? ChannelType.DirectMessage : ChannelType.Channel,
									UserChannel = await this.GetUserChannel(slackMessage, connection),
									BotName = connection.Self.Name,
									BotId = connection.Self.Id,
									BotIsMentioned = slackMessage.MentionsBot,
									TraceId = traceId
								};

			return incomingMessage;
		}

		public async Task<SlackChatHub> GetChatHub(ResponseMessage responseMessage, ISlackConnection connection)
		{
			SlackChatHub chatHub = null;

			if (responseMessage.ChannelType == ChannelType.Channel)
			{
				chatHub = new SlackChatHub { Id = responseMessage.Channel };
			}
			else if (responseMessage.ChannelType == ChannelType.DirectMessage)
			{
				if (string.IsNullOrEmpty(responseMessage.Channel))
				{
					chatHub = await this.GetUserChatHub(responseMessage.UserId, connection);
				}
				else
				{
					chatHub = new SlackChatHub { Id = responseMessage.Channel };
				}
			}

			return chatHub;
		}

		public IList<SlackAttachment> ConvertAttachmentsToSlackAttachments(IEnumerable<Attachment> attachments)
		{
			var slackAttachments = new List<SlackAttachment>();

			if (attachments != null)
			{
				foreach (var attachment in attachments)
				{
					slackAttachments.Add(new SlackAttachment
					{
						Text = attachment.Text,
						Title = attachment.Title,
						Fallback = attachment.Fallback,
						ImageUrl = attachment.ImageUrl,
						ThumbUrl = attachment.ThumbUrl,
						AuthorName = attachment.AuthorName,
						ColorHex = attachment.Color,
						Fields = GetAttachmentFields(attachment)
					});
				}
			}

			return slackAttachments;
		}

		private static IList<SlackAttachmentField> GetAttachmentFields(Attachment attachment)
		{
			var attachmentFields = new List<SlackAttachmentField>();

			if (attachment?.AttachmentFields != null)
			{
				foreach (var attachmentField in attachment.AttachmentFields)
				{
					attachmentFields.Add(new SlackAttachmentField
										{
											Title = attachmentField.Title,
											Value = attachmentField.Value,
											IsShort = attachmentField.IsShort
										});
				}
			}

			return attachmentFields;
		}

		private async Task<string> GetUserChannel(SlackMessage message, ISlackConnection connection)
		{
			return (await this.GetUserChatHub(message.User.Id, connection, joinChannel: false) ?? new SlackChatHub()).Id;
		}

		private async Task<SlackChatHub> GetUserChatHub(string userId, ISlackConnection connection, bool joinChannel = true)
		{
			SlackChatHub chatHub = null;

			if (connection.UserCache.ContainsKey(userId))
			{
				var username = "@" + connection.UserCache[userId].Name;
				chatHub = connection.ConnectedDMs().FirstOrDefault(x => x.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
			}

			if (chatHub == null && joinChannel)
			{
				chatHub = await connection.JoinDirectMessageChannel(userId);
			}

			return chatHub;
		}

		private string GetUsername(SlackUser user, ISlackConnection connection)
		{
			return connection.UserCache.ContainsKey(user.Id) ? connection.UserCache[user.Id].Name : string.Empty;
		}
	}
}
