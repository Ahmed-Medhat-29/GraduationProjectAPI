using System;
using GraduationProjectAPI.Models.Reviews;
using GraduationProjectAPI.Utilities.StaticStrings;

namespace GraduationProjectAPI.DTOs
{
	public class ReviewDto
	{
		public string Name { get; set; }
		public DateTime DateReviewed { get; set; }
		public bool IsWorthy { get; set; }
		public string ImageUrl { get; set; }

		public ReviewDto(MediatorReview review)
		{
			DateReviewed = review.DateReviewed;
			IsWorthy = review.IsWorthy;
			ImageUrl = Paths.ProfilePicture(review.ReviewerId);
		}

		public ReviewDto(CaseReview review)
		{
			DateReviewed = review.DateReviewed;
			IsWorthy = review.IsWorthy;
			ImageUrl = Paths.ProfilePicture(review.MediatorId);
		}
	}
}
