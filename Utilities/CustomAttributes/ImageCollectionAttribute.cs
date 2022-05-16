using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Utilities.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class ImageCollectionAttribute : ValidationAttribute
	{
		public long MaxSize { get; set; }

		public override bool IsValid(object value)
		{
			if (value == null)
				return true;

			var files = value as IFormFileCollection ?? throw new InvalidCastException("Object must be of type IFormFileCollection");
			if (!IsSizeValid(files))
			{
				ErrorMessage = $"One or more images exceeded size limit of {MaxSize} bytes";
				return false;
			}

			if (!IsExtensionValid(files))
			{
				ErrorMessage = "One or more images are not valid";
				return false;
			}

			return true;
		}

		private bool IsSizeValid(IFormFileCollection files)
		{
			return MaxSize <= 0 || files.All(f => f.Length < MaxSize);
		}

		private bool IsExtensionValid(IFormFileCollection files)
		{
			return files.All(f => ValidImage.Extentions.Contains(Path.GetExtension(f.FileName.ToLower())));
		}
	}
}
