using System;

namespace GraduationProjectAPI.DTOs.Response
{
	public class ReviewElementDto
	{
		public string Name { get; set; }
		public bool IsWorthy { get; set; }
		public DateTime DateReviewed { get; set; }
		public string ImageUrl { get; set; }
	}
}
