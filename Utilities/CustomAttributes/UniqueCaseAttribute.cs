using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request.Cases;
using Microsoft.Extensions.DependencyInjection;

namespace GraduationProjectAPI.Utilities.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class UniqueCaseAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var dto = value as NewCaseDto ?? throw new InvalidCastException($"Object must be of type {nameof(NewCaseDto)}");
			var context = validationContext.GetService<ApplicationDbContext>();
			var @case = context.Cases
				.Select(m => new { m.NationalId, m.PhoneNumber })
				.FirstOrDefault(m => m.PhoneNumber == dto.PhoneNumber ||
									 m.NationalId == dto.NationalId);

			if (@case == null)
				return ValidationResult.Success;

			ValidationResult result;

			if (@case.PhoneNumber == dto.PhoneNumber)
				result = new ValidationResult("Phone number already exists", new[] { nameof(dto.PhoneNumber) });
			else
				result = new ValidationResult("National ID already exists", new[] { nameof(dto.NationalId) });

			return result;
		}
	}
}
