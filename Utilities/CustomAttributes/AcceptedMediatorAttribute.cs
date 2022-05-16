using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request.Mediators;
using GraduationProjectAPI.Enums;

namespace GraduationProjectAPI.Utilities.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class AcceptedMediatorAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var dto = value as SignInRequestDto;
			var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
			var statusType = context.Mediators
				.Where(m => m.PhoneNumber == dto.PhoneNumber)
				.Select(m => m.StatusId)
				.FirstOrDefault();

			if (statusType == StatusType.Accepted)
				return ValidationResult.Success;

			if (statusType == 0)
				return new ValidationResult("Please register first");

			if (statusType == StatusType.Pending || statusType == StatusType.Submitted)
				return new ValidationResult("Your registeration request is pending...");

			if (statusType == StatusType.Rejected)
				return new ValidationResult("Your registeration request has been rejected");

			return new ValidationResult("Something went wrong :(");
		}
	}
}
