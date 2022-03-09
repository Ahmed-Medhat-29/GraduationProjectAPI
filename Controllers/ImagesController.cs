using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProjectAPI.DTOs.Mediator;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProjectAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		[Authorize]
		[HttpGet("Mediator/[action]/{imageName}")]
		[ProducesResponseType(typeof(MediatorProfile), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Profile(string imageName)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId != Path.GetFileNameWithoutExtension(imageName))
				return NotFound(null);

			var imagePath = ImagePath.Profile + userId + Path.GetExtension(imageName);
			if (!new FileInfo(imagePath).Exists)
				return NotFound(null);

			var image = await System.IO.File.ReadAllBytesAsync(imagePath);
			return File(image, "image/jpeg");
		}
	}
}
