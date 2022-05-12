using System.Collections.Generic;

namespace GraduationProjectAPI.DTOs.Response.Payments
{
	public class WalletDto
	{
		public string Name { get; set; }
		public int Balance { get; set; }
		public string ImageUrl { get; set; }
		public IEnumerable<TransactionElement> Transactions { get; set; }
	}
}
