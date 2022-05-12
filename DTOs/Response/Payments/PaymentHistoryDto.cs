using System.Collections.Generic;

namespace GraduationProjectAPI.DTOs.Response.Payments
{
	public class PaymentHistoryDto
	{
		public int Total { get; set; }
		public int Paid { get; set; }
		public int Remaining { get; set; }
		public IEnumerable<PaymentHistoryElementDto> History { get; set; }
	}
}
