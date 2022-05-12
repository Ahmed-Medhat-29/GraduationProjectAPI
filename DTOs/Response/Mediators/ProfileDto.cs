using GraduationProjectAPI.Enums;

namespace GraduationProjectAPI.DTOs.Response.Mediators
{
	public class ProfileDto
	{
		public string Name { get; set; }
		public int Balance { get; set; }
		public string Bio { get; set; }
		public SocialStatusType SocialStatusId { get; set; }
		public string ImageUrl { get; set; }
	}
}
