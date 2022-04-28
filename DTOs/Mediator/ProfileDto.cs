using GraduationProjectAPI.Utilities.StaticStrings;

namespace GraduationProjectAPI.DTOs.Mediator
{
	public class ProfileDto
	{
		public string Name { get; set; }
		public string Bio { get; set; }
		public byte SocialStatusId { get; set; }
		public string ImageUrl { get; set; }

		public ProfileDto(Models.Mediator mediator)
		{
			Name = mediator.Name;
			Bio = mediator.Bio;
			SocialStatusId = mediator.SocialStatusId;
			ImageUrl = Paths.ProfilePicture(mediator.Id);
		}
	}
}
