using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.Models.Reviews
{
	public class CaseReview
	{
		public Case Case { get; set; }
		public int CaseId { get; set; }

		public Mediator Mediator { get; set; }
		public int MediatorId { get; set; }

		public bool IsWorthy { get; set; }

		[Required, MaxLength(4000)]
		public string Description { get; set; }

		public CaseReview()
		{

		}

		public CaseReview(int mediatorId)
		{
			MediatorId = mediatorId;
		}
	}
}
