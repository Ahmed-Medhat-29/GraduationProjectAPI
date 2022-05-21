using System.Collections.Generic;

namespace GraduationProjectAPI.DTOs.Response.Payments
{
	public class CasePaymentHistoryDto
	{
		public int Total { get; set; }
		public int Paid { get; set; }
		public int RoundNumber { get; set; }
		public IEnumerable<PaymentElementDto> History { get; set; }
	}
}
