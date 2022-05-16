namespace GraduationProjectAPI.DTOs.Response.Payments
{
	public class CasePaymentElementDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int Paid { get; set; }
		public int Amount { get; set; }
		public int Total { get; set; }
		public string PhoneNumber { get; set; }
		public string MediatorName { get; set; }
		public string CaseName { get; set; }
		public int Contributers { get; set; }
		public short Age { get; set; }
		public string ImageUrl { get; set; }
	}
}
