using System.ComponentModel.DataAnnotations;
using GraduationProjectAPI.Models.Reviews;

namespace GraduationProjectAPI.DTOs
{
	public class NewReviewDto
	{
		[Range(1, int.MaxValue)]
		public int RevieweeId { get; set; }

		public bool IsWorthy { get; set; }

		[Required, MaxLength(4000)]
		public string Description { get; set; }

		public MediatorReview ToMediatorReview(int mediatorId)
		{
			return new MediatorReview
			{
				RevieweeId = RevieweeId,
				ReviewerId = mediatorId,
				IsWorthy = IsWorthy,
				Description = Description
			};
		}

		public CaseReview ToCaseReview(int mediatorId)
		{
			return new CaseReview
			{
				CaseId = RevieweeId,
				MediatorId = mediatorId,
				IsWorthy = IsWorthy,
				Description = Description
			};
		}
	}
}
