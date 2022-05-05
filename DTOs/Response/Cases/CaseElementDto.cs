namespace GraduationProjectAPI.DTOs.Response.Cases
{
	public class CaseElementDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Priority { get; set; }
		public int FundRaised { get; set; }
		public int TotalNeeded { get; set; }
		public int NumberOfContributer { get; set; }
		public short Age { get; set; }
		public string ImageUrl { get; set; }
	}
}
