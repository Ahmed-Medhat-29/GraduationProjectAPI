using System.ComponentModel.DataAnnotations;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.CustomAttributes;

namespace GraduationProjectAPI.DTOs.Request.Donators
{
	[UniqueDonator]
	public class RegisterDto
	{
		[Required, MinLength(2), MaxLength(250)]
		public string Name { get; set; }

		[Required, MinLength(11), MaxLength(11)]
		[RegularExpression("^[0-9]+$", ErrorMessage = "Phone number must be only numbers")]
		public string PhoneNumber { get; set; }

		[Required, MaxLength(4000)]
		public string FirebaseToken { get; set; }

		public Donator ToDonator()
		{
			return new Donator
			{
				Name = Name,
				PhoneNumber = PhoneNumber,
				FirebaseToken = FirebaseToken
			};
		}
	}
}
