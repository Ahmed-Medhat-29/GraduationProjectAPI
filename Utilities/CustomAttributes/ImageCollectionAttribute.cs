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
			foreach (var file in files)
			{
				if (!ImageExtensions.Extentions.Contains(Path.GetExtension(file.FileName.ToLower())))
				{
					ErrorMessage = "One or more images are not valid";
					return false;
				}

				if (MaxSize > 0 && file.Length > MaxSize)
				{
					ErrorMessage = $"One or more images exceeded size limit of {MaxSize} bytes";
					return false;
				}
			}

			return true;
		}
	}
}
