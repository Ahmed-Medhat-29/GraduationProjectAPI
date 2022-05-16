using System;
using GraduationProjectAPI.Models;

namespace GraduationProjectAPI.DTOs.Response.Chats
{
	public class ChatDto
	{
		public string Message { get; set; }
		public DateTime DateTime { get; set; }
		public string Type { get; set; }

		public ChatDto()
		{

		}

		public ChatDto(Chat chat)
		{
			Message = chat.Message;
			DateTime = chat.DateTime;
			Type = chat.MessageTypeId.ToString();
		}
	}
}
