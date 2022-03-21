using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.DTOs.Mediator
{
	public class MediatorRegister
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

		[Required]
		public IFormFile NatoinalIdImage { get; set; }

		[Range(1, 2)]
		public byte GenderId { get; set; }

		[Range(1, 4)]
		public byte SocialStatusId { get; set; }
	}
}
