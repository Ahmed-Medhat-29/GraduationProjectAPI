using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.CustomAttributes;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.DTOs.Mediator
{
	public class RegisterDto
	{
		[Required, MaxLength(250), MinLength(2)]
		public string Name { get; set; }

		[Required, MaxLength(11), MinLength(11), RegularExpression("^[0-9]+$", ErrorMessage = "Phone number must be only numbers")]
		public string PhoneNumber { get; set; }

		[Required]
		public GeoLocationDto GeoLocation { get; set; }

		[Required, MaxLength(14), MinLength(14), RegularExpression("^[0-9]+$", ErrorMessage = "National id must be only numbers")]
		public string NationalId { get; set; }

		[Required, MaxLength(4000)]
		public string FirebaseToken { get; set; }

		[Required, ImageFile(MaxSize = 1024 * 1024)]
		public IFormFile ProfileImage { get; set; }

		[Required, ImageFile(MaxSize = 1024 * 1024)] // 1MB
		public IFormFile NationalIdImage { get; set; }

		[Range(1, 2)]
		public byte GenderId { get; set; }

		[Range(1, 4)]
		public byte SocialStatusId { get; set; }

		public async Task SetImagesAsync(Models.Mediator mediator)
		{
			await MediatorImagesHandler.SetProfileImageAsync(mediator, ProfileImage);
			await MediatorImagesHandler.SetNationalIdImageAsync(mediator, NationalIdImage);
		}
	}
}
