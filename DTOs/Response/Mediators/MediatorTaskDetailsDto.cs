using System;
using System.Collections.Generic;

namespace GraduationProjectAPI.DTOs.Response.Mediators
{
	public class MediatorTaskDetailsDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime? BirthDate { get; set; }
		public string ImageUrl { get; set; }
		public IEnumerable<ReviewDto> Reviews { get; set; }
	}
}
