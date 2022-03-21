using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Models.CaseProperties
{
	public class Image
	{
		public int Id { get; set; }

		[Required]
		public byte[] Data { get; set; }

		public Case Case { get; set; }
		public int CaseId { get; set; }

		public Image()
		{

		}

		public Image(IFormFile image)
		{
			using (var stream = new MemoryStream())
			{
				image.CopyTo(stream);
				Data = stream.ToArray();
			}
		}
	}
}
