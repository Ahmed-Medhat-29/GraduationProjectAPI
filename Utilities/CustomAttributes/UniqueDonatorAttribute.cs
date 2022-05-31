using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request.Donators;
using Microsoft.Extensions.DependencyInjection;

namespace GraduationProjectAPI.Utilities.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class UniqueDonatorAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var dto = value as RegisterDto ?? throw new InvalidCastException($"Object must be of type {nameof(RegisterDto)}"); ;
			var context = validationContext.GetService<ApplicationDbContext>();
			var phoneNumber = context.Donators.Select(m => m.PhoneNumber)
				.FirstOrDefault(num => num == dto.PhoneNumber);

			if (string.IsNullOrWhiteSpace(phoneNumber))
				return ValidationResult.Success;

			return new ValidationResult("Phone number already exists", new[] { nameof(dto.PhoneNumber) });
		}
	}
}
