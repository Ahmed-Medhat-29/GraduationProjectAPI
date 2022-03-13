using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.DTOs.Case
{
	public class CaseDto
	{
		[Required]
		public CaseResponsibilityHolder ResponsibilityHolder { get; set; }

		public string Title { get; set; }

		public string Story { get; set; }

		public int NeededMoneyAmount { get; set; }

		public DateTime? DateLimit { get; set; }

		public byte Adults { get; set; }

		public byte Children { get; set; }

		public byte RelationshipId { get; set; }

		public GeoLocationDto GeoLocation { get; set; }

		public string Address { get; set; }

		public int RegionId { get; set; }

		public byte CategoryId { get; set; }

		public byte PriorityId { get; set; }

		public IEnumerable<IFormFile> OptionalImages { get; set; }
	}
}
