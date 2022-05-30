using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request.Donators;

namespace GraduationProjectAPI.Utilities.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class RegisteredDonatorAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var dto = value as SignInRequestDto;
			var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
			var IsRegistered = context.Donators.Any(m => m.PhoneNumber == dto.PhoneNumber);

			if (IsRegistered)
				return ValidationResult.Success;

			return new ValidationResult("Please register first");
		}
	}
}
