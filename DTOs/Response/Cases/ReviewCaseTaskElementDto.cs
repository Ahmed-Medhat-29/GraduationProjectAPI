namespace GraduationProjectAPI.DTOs.Response.Cases
{
	public class ReviewCaseTaskElementDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int NeededMoneyAmount { get; set; }
		public short Age { get; set; }
		public string Period { get; set; }
		public string Details { get; set; }
		public string ImageUrl { get; set; }
	}
}
