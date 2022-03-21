﻿using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.DTOs.Mediator
{
	public class MediatorPhoneNumber
	{
		[Required, MaxLength(11), MinLength(11), RegularExpression("^[0-9]+$", ErrorMessage = "Phone number must be only numbers")]
		public string PhoneNumber { get; set; }
	}
}