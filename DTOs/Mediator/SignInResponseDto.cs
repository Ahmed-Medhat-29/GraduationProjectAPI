using System;
using GraduationProjectAPI.Utilities.StaticStrings;

namespace GraduationProjectAPI.DTOs.Mediator
{
	public class SignInResponseDto
	{
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string NationalId { get; set; }
		public string Job { get; set; }
		public string Address { get; set; }
		public DateTime? BirthDate { get; set; }
		public string Bio { get; set; }
		public string Region { get; set; }
		public string Gender { get; set; }
		public string SocialStatus { get; set; }
		public string Locale { get; set; }
		public bool Completed { get; set; }
		public string Status { get; set; }
		public string FirebaseToken { get; set; }
		public string JwtToken { get; set; }
		public string ProfileImageUrl { get; set; }
		public string NationalIdImageUrl { get; set; }
		public GeoLocationDto GeoLocation { get; set; }

		public SignInResponseDto(Models.Mediator mediator)
		{
			Name = mediator.Name;
			PhoneNumber = mediator.PhoneNumber;
			NationalId = mediator.NationalId;
			Job = mediator.Job;
			Address = mediator.Address;
			BirthDate = mediator.BirthDate;
			Bio = mediator.Bio;
			Completed = mediator.Completed;
			FirebaseToken = mediator.FirebaseToken;
			ProfileImageUrl = Paths.ProfilePicture(mediator.Id);
			NationalIdImageUrl = Paths.NationalIdImage(mediator.Id);
		}
	}
}
