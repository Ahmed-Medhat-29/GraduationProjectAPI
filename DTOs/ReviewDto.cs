using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.DTOs
{
	public class ReviewDto
	{
		[Range(1, int.MaxValue)]
		public int RevieweeId { get; set; }

		public bool IsWorthy { get; set; }

		[Required, MaxLength(4000)]
		public string Description { get; set; }
	}
}
