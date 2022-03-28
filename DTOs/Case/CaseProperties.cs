using System.Collections.Generic;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Models.Shared;

namespace GraduationProjectAPI.DTOs.Case
{
	public class CaseProperties
	{
		public IEnumerable<Gender> Genders { get; set; }
		public IEnumerable<SocialStatus> SocialStatus { get; set; }
		public IEnumerable<Relationship> Relationships { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Period> Periods { get; set; }
		public IEnumerable<Priority> Priorities { get; set; }
	}
}
