using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.DTOs.Mediator
{
	public class MediatorRegister
	{
		[Required, MaxLength(200), MinLength(2)]
		public string Name { get; set; }

		[Required, MaxLength(11), MinLength(11), RegularExpression("^[0-9]+$", ErrorMessage = "Phone number must be only numbers")]
		public string PhoneNumber { get; set; }

		[Required]
		public GeoLocationDTO GeoLocation { get; set; }

		[Required, MaxLength(14), MinLength(14), RegularExpression("^[0-9]+$", ErrorMessage = "National id must be only numbers")]
		public string NationalId { get; set; }

		[Required]
		public IFormFile NatoinalIdImage { get; set; }

		[Required]
		public byte? GenderId { get; set; }

		[Required]
		public byte? SocialStatusId { get; set; }
	}
}
