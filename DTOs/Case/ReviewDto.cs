using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.DTOs.Case
{
	public class ReviewDto
	{
		[Range(1, int.MaxValue)]
		public int CaseId { get; set; }

		public bool IsWorthy { get; set; }

		[Required, MaxLength(4000)]
		public string Description { get; set; }
	}
}
