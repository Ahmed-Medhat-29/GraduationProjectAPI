using System;

namespace GraduationProjectAPI.DTOs.Response.Payments
{
	public class TransactionElement
	{
		public int CaseId { get; set; }
		public string Name { get; set; }
		public DateTime DateTime { get; set; }
		public int Amount { get; set; }
		public string ImageUrl { get; set; }
	}
}
