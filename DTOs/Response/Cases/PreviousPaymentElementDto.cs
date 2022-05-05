using System;

namespace GraduationProjectAPI.DTOs.Response.Cases
{
	public class PreviousPaymentElementDto
	{
		public string Name { get; set; }
		public DateTime Datetime { get; set; }
		public int Amount { get; set; }
		public string ImageUrl { get; set; }
	}
}
