using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProjectAPI.Models.Reviews
{
	public class MediatorReview
	{
		[ForeignKey(nameof(RevieweeId))]
		public Mediator Reviewee { get; set; }
		public int RevieweeId { get; set; }

		[ForeignKey(nameof(ReviewerId))]
		public Mediator Reviewer { get; set; }
		public int ReviewerId { get; set; }

		public bool IsWorthy { get; set; }

		[Required, MaxLength(4000)]
		public string Description { get; set; }

		[Column(TypeName = "date")]
		public DateTime DateReviewed { get; private set; } = DateTime.Now;
	}
}
