using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace GraduationProjectAPI.Utilities.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class UniqueMediatorAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var dto = value as RegisterDto ?? throw new InvalidCastException($"Object must be of type {nameof(RegisterDto)}"); ;
			var context = validationContext.GetService<ApplicationDbContext>();
			var mediator = context.Mediators
				.Select(m => new { m.NationalId, m.PhoneNumber })
				.FirstOrDefault(m => m.PhoneNumber == dto.PhoneNumber ||
									 m.NationalId == dto.NationalId);

			if (mediator == null)
				return ValidationResult.Success;

			ValidationResult result;

			if (mediator.PhoneNumber == dto.PhoneNumber)
				result = new ValidationResult("Phone number already exists", new[] { nameof(dto.PhoneNumber) });
			else
				result = new ValidationResult("National ID already exists", new[] { nameof(dto.NationalId) });

			return result;
		}
	}
}
