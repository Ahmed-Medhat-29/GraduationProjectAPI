using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.DTOs.Request.Payments
{
	public class CasePaymentConfirmationDto
	{
		[Range(1, int.MaxValue)]
		public int Id { get; set; }

		[MaxLength(4000)]
		public string Details { get; set; }
	}
}
