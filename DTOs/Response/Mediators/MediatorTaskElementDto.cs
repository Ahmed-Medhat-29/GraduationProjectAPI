using System;

namespace GraduationProjectAPI.DTOs.Response.Mediators
{
	public class MediatorTaskElementDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime DateRegistered { get; set; }
		public string Details { get; set; }
		public string ImageUrl { get; set; }
	}
}
