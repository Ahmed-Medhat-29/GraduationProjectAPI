using System;
using GraduationProjectAPI.Models;

namespace GraduationProjectAPI.DTOs.Response.Chats
{
	public class ChatResponseDto
	{
		public string Message { get; set; }
		public DateTime DateTime { get; set; }
		public string Type { get; set; }

		public ChatResponseDto(Chat chat)
		{
			Message = chat.Message;
			DateTime = chat.DateTime;
			Type = chat.MessageTypeId.ToString();
		}
	}
}
