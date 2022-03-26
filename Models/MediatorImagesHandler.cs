using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Models
{
	public static class MediatorImagesHandler
	{
		public static async Task SetNationalIdImageAsync(Mediator mediator, IFormFile file)
		{
			using (var stream = new MemoryStream())
			{
				await file.CopyToAsync(stream);
				mediator.NationalIdImage = stream.ToArray();
			}
		}

		public static async Task SetProfileImageAsync(Mediator mediator, IFormFile file)
		{
			using (var stream = new MemoryStream())
			{
				await file.CopyToAsync(stream);
				mediator.ProfileImage = stream.ToArray();
			}
		}
	}
}
