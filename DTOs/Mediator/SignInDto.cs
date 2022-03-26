using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.DTOs.Mediator
{
	public class SignInDto
	{
		[Required, MaxLength(11), MinLength(11), RegularExpression("^[0-9]+$", ErrorMessage = "Phone number must be only numbers")]
		public string PhoneNumber { get; set; }

		[Required, MaxLength(20)]
		public string IMEI { get; set; }

		[Required, MaxLength(4000)]
		public string FirebaseToken { get; set; }
	}
}
