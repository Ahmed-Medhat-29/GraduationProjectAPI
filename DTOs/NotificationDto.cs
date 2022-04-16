namespace GraduationProjectAPI.DTOs
{
	public class NotificationDto
	{
		public string Title { get; set; }
		public string Body { get; set; }

		public NotificationDto(string title, string body)
		{
			Title = title;
			Body = body;
		}
	}
}
