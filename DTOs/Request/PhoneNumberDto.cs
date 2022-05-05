using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.DTOs.Request
{
	public class PhoneNumberDto
	{
		private const string _errorMessage = "Phone number must be 11 digits";

		[Required]
		[MaxLength(11, ErrorMessage = _errorMessage)]
		[MinLength(11, ErrorMessage = _errorMessage)]
		[RegularExpression("^[0-9]+$", ErrorMessage = _errorMessage)]
		public string PhoneNumber { get; set; }
	}
}
