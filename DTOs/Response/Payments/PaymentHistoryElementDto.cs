using System;

namespace GraduationProjectAPI.DTOs.Response.Payments
{
	public class PaymentHistoryElementDto
	{
		public string Name { get; set; }
		public DateTime Datetime { get; set; }
		public int Amount { get; set; }
		public string ImageUrl { get; set; }
	}
}
