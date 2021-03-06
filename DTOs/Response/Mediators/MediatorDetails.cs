using System;
using GraduationProjectAPI.DTOs.Common;

namespace GraduationProjectAPI.DTOs.Response.Mediators
{
	public class MediatorDetails
	{
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string NationalId { get; set; }
		public int Balance { get; set; }
		public string Job { get; set; }
		public string Address { get; set; }
		public DateTime? BirthDate { get; set; }
		public string Bio { get; set; }
		public bool Completed { get; set; }
		public string JwtToken { get; set; }
		public string FirebaseToken { get; set; }
		public string Gender { get; set; }
		public string Region { get; set; }
		public int? RegionId { get; set; }
		public string SocialStatus { get; set; }
		public string Locale { get; set; }
		public string Status { get; set; }
		public string ProfileImageUrl { get; set; }
		public string NationalIdImageUrl { get; set; }
		public GeoLocationDto GeoLocation { get; set; }
	}
}
