using System.ComponentModel.DataAnnotations;
using GraduationProjectAPI.Enums;

namespace GraduationProjectAPI.DTOs.Chat
{
	public class ChatDto
	{
		public int ChatId { get; set; }

		[Required, MaxLength(4000)]
		public string Message { get; set; }

		public Models.Chat ToChat(int mediatorId, MessageType type)
		{
			return new Models.Chat
			{
				ChatId = ChatId,
				Message = Message,
				MediatorId = mediatorId,
				MessageTypeId = (byte)type
			};
		}
	}
}
