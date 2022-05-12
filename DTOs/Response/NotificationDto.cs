using System;

namespace GraduationProjectAPI.DTOs.Response
{
	public class NotificationDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public bool IsRead { get; set; }
		public int TaskId { get; set; }
		public string Type { get; set; }
		public DateTime DateTime { get; set; }
		public string ImageUrl { get; set; }

		public NotificationDto()
		{

		}

		public NotificationDto(Models.Notification notification)
		{
			Id = notification.Id;
			Title = notification.Title;
			Body = notification.Body;
			IsRead = notification.IsRead;
			TaskId = notification.TaskId;
			DateTime = notification.DateTime;
			ImageUrl = notification.ImageUrl;
			Type = notification.TypeId.ToString();
		}
	}
}
