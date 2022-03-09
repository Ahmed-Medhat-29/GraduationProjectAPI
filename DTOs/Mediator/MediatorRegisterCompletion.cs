using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.DTOs.Mediator
{
	public class MediatorRegisterCompletion
	{
		[MaxLength(200), MinLength(2)]
		public string Job { get; set; }

		[MaxLength(2000), MinLength(3)]
		public string Address { get; set; }

		public DateTime? BirthDate { get; set; }

		[MaxLength(4000), MinLength(3)]
		public string Bio { get; set; }

		public IFormFile ProfileImage { get; set; }

		[Required]
		public int? RegionId { get; set; }

		public async Task UpdateMediatorAsync(Models.Mediator mediator)
		{
			var profileImageTask = mediator.SetProfileImageAsync(ProfileImage);
			mediator.Job = Job ?? mediator.Job;
			mediator.Address = Address ?? mediator.Address;
			mediator.BirthDate = BirthDate ?? mediator.BirthDate;
			mediator.Bio = Bio ?? mediator.Bio;
			mediator.RegionId = RegionId ?? mediator.RegionId;
			await profileImageTask;
		}
	}
}
