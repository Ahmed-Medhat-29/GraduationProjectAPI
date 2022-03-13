using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Utilities.CustomValidationAttributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class RequiredFile : ValidationAttribute
	{
		private const string _errorMessage = "The field is required";
		public override bool IsValid(object value)
		{
			var file = value as IFormFile;
			return file != null && file.Length > 0;
		}

		public override string FormatErrorMessage(string name)
		{
			return $"The field {name} is required";
		}
	}
}
