using System.ComponentModel.DataAnnotations;
using GraduationProjectAPI.DTOs.Common;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Utilities.CustomAttributes;
using GraduationProjectAPI.Utilities.General;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.DTOs.Request.Mediators
{
	[UniqueMediator]
	public class RegisterDto
	{
		[Required, MinLength(2), MaxLength(250)]
		public string Name { get; set; }

		[Required, MinLength(11), MaxLength(11)]
		[RegularExpression("^[0-9]+$", ErrorMessage = "Phone number must be only numbers")]
		public string PhoneNumber { get; set; }

		[Required]
		public GeoLocationDto GeoLocation { get; set; }

		[Required, MaxLength(14), MinLength(14)]
		[RegularExpression("^[0-9]+$", ErrorMessage = "National ID must be only numbers")]
		public string NationalId { get; set; }

		[Required, MaxLength(4000)]
		public string FirebaseToken { get; set; }

		[Required, ImageFile(MaxSize = 1024 * 1024)]
		public IFormFile ProfileImage { get; set; }

		[Required, ImageFile(MaxSize = 1024 * 1024)]
		public IFormFile NationalIdImage { get; set; }

		[Required]
		public GenderType? GenderId { get; set; }

		[Required]
		public SocialStatusType? SocialStatusId { get; set; }

		public Models.Mediator ToMediator()
		{
			return new Models.Mediator
			{
				Name = Name,
				PhoneNumber = PhoneNumber,
				NationalId = NationalId,
				FirebaseToken = FirebaseToken,
				GeoLocation = GeoLocation.ToGeoLocation(),
				ProfileImage = FormFileHandler.ConvertToBytes(ProfileImage),
				NationalIdImage = FormFileHandler.ConvertToBytes(NationalIdImage),
				GenderId = (GenderType)GenderId,
				SocialStatusId = (SocialStatusType)SocialStatusId,
				StatusId = StatusType.Pending
			};
		}
	}
}
