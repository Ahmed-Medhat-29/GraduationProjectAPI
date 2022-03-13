﻿using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.DTOs.Mediator
{
	public class MediatorSignIn
	{
		[Required, MaxLength(11), MinLength(11), RegularExpression("^[0-9]+$", ErrorMessage = "Phone number must be only numbers")]
		public string PhoneNumber { get; set; }

		[Required, MaxLength(15), MinLength(15), RegularExpression("^[0-9]+$", ErrorMessage = "IMEI must be only numbers")]
		public string IMEI { get; set; }

		[Required]
		public string FirebaseToken { get; set; }
	}
}
