using System;

namespace GraduationProjectAPI.DTOs.Reviews
{
	public class PendingMediatorDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public DateTime BirthDate { get; set; }
		public string ImageUrl { get; set; }
		public ReviewDto[] Reviews { get; set; }

		public PendingMediatorDto(Models.Mediator mediator)
		{

		}
	}
}
