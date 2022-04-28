using System.ComponentModel.DataAnnotations;
using GraduationProjectAPI.Models.Reviews;

namespace GraduationProjectAPI.DTOs
{
	public class ReviewDto
	{
		[Range(1, int.MaxValue)]
		public int RevieweeId { get; set; }

		public bool IsWorthy { get; set; }

		[Required, MaxLength(4000)]
		public string Description { get; set; }

		public MediatorReview ToMediatorReview()
		{
			return new MediatorReview
			{
				RevieweeId = RevieweeId,
				IsWorthy = IsWorthy,
				Description = Description
			};
		}

		public CaseReview ToCaseReview()
		{
			return new CaseReview
			{
				CaseId = RevieweeId,
				IsWorthy = IsWorthy,
				Description = Description
			};
		}
	}
}
