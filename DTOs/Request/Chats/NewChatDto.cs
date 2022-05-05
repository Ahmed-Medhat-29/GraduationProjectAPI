using System.ComponentModel.DataAnnotations;
using GraduationProjectAPI.Enums;

namespace GraduationProjectAPI.DTOs.Request.Chats
{
	public class NewChatDto
	{
		[Required, MaxLength(4000)]
		public string Message { get; set; }

		public Models.Chat ToChat(int mediatorId, MessageType type)
		{
			return new Models.Chat
			{
				Message = Message,
				MediatorId = mediatorId,
				MessageTypeId = type
			};
		}
	}
}
