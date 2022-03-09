using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.Models.CaseProperties
{
	public class Image
	{
		public int Id { get; set; }

		[Required, MaxLength(4000)]
		public string Path { get; set; }

		public Case Case { get; set; }
		public int CaseId { get; set; }
	}
}
