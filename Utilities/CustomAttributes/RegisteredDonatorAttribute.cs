using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request.Donators;
using Microsoft.Extensions.DependencyInjection;

namespace GraduationProjectAPI.Utilities.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class RegisteredDonatorAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var dto = value as SignInRequestDto ?? throw new InvalidCastException($"Object must be of type {nameof(SignInRequestDto)}");
			var context = validationContext.GetService<ApplicationDbContext>();
			var isRegistered = context.Donators.Any(m => m.PhoneNumber == dto.PhoneNumber);

			if (isRegistered)
				return ValidationResult.Success;

			return new ValidationResult("Please register first");
		}
	}
}
