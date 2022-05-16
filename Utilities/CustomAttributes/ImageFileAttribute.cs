using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Utilities.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class ImageFileAttribute : ValidationAttribute
	{
		public long MaxSize { get; set; }

		public override bool IsValid(object value)
		{
			if (value == null)
				return true;

			var file = value as IFormFile ?? throw new InvalidCastException("Object must be of type IFormFile");
			if (!IsSizeValid(file))
			{
				ErrorMessage = $"Image exceeded size limit of {MaxSize} bytes";
				return false;
			}

			if (!IsExtensionValid(file))
			{
				ErrorMessage = "Image is not valid";
				return false;
			}

			return true;
		}

		private bool IsSizeValid(IFormFile file)
		{
			return MaxSize <= 0 || file.Length < MaxSize;
		}

		private bool IsExtensionValid(IFormFile file)
		{
			return ValidImage.Extentions.Contains(Path.GetExtension(file.FileName.ToLower()));
		}
	}
}
