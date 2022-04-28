using System;
using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.DTOs.Mediator
{
	public class CompleteProfileDto
	{
		[MaxLength(250), MinLength(2)]
		public string Job { get; set; }

		[Required, MaxLength(4000), MinLength(3)]
		public string Address { get; set; }

		public DateTime BirthDate { get; set; }

		[MaxLength(4000), MinLength(3)]
		public string Bio { get; set; }

		[Range(1, int.MaxValue)]
		public int RegionId { get; set; }

		public void UpdateMediator(Models.Mediator mediator)
		{
			mediator.Job = Job ?? mediator.Job;
			mediator.Address = Address ?? mediator.Address;
			mediator.BirthDate = BirthDate;
			mediator.Bio = Bio ?? mediator.Bio;
			mediator.RegionId = RegionId;
			mediator.Completed = true;
		}
	}
}
