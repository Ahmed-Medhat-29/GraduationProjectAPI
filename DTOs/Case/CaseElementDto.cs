using System.Collections.Generic;

namespace GraduationProjectAPI.DTOs.Case
{
	public class CaseElementDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Priority { get; set; }
		public int FundRaised { get; set; }
		public short Age { get; set; }
		public IEnumerable<string> ImagesUrl { get; set; }
	}
}
