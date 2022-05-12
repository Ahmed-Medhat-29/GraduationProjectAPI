using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.DTOs.Response.Cases
{
	public class CaseTaskDetailsDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int NeededMoneyAmount { get; set; }
		public DateTime DateRequested { get; set; }
		public string Story { get; set; }
		public string Period { get; set; }
		public IEnumerable<string> ImagesUrls { get; set; }
		public CaseMediatorDto Mediator { get; set; }
		public IEnumerable<ReviewElementDto> Reviews { get; set; }
	}
}
