using System.Threading.Tasks;
using GraduationProjectAPI.Utilities;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Models
{
	public class MediatorImagesHandler
	{
		private readonly Mediator _mediator;

		public MediatorImagesHandler(Mediator mediator)
		{
			_mediator = mediator;
		}

		public async Task SetMediatorNationalIdImageAsync(IFormFile file)
		{
			var imageName = await AssignImageAsync(ImagePath.NationalId, file);
			if (!string.IsNullOrWhiteSpace(imageName))
				_mediator.NationalIdImageName = imageName;
		}

		public async Task SetMediatorProfileImageAsync(IFormFile file)
		{
			var imageName = await AssignImageAsync(ImagePath.Profile, file);
			if (!string.IsNullOrWhiteSpace(imageName))
				_mediator.ProfileImageName = imageName;
		}

		private async Task<string> AssignImageAsync(string path, IFormFile file)
		{
			return await new FileManager(path, file).SaveAsync(_mediator.Id.ToString());
		}
	}
}
