using System;
using System.Collections.Generic;
using GraduationProjectAPI.DTOs.Response.Payments;

namespace GraduationProjectAPI.DTOs.Response.Cases
{
	public class CaseInfoDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Story { get; set; }
		public DateTime Datetime { get; set; }
		public int TotalNeeded { get; set; }
		public CaseMediatorDto Mediator { get; set; }
		public IEnumerable<string> ImagesUrls { get; set; }
		public IEnumerable<PaymentHistoryElementDto> History { get; set; }
	}
}
