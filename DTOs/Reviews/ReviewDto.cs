using System;

namespace GraduationProjectAPI.DTOs.Reviews
{
	public class ReviewDto
	{
		public string Name { get; set; }
		public DateTime DateReviewed { get; set; }
		public bool IsWorthy { get; set; }
		public string ImageUrl { get; set; }
	}
}
