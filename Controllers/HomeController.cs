using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.ExtensionMethods;
using GraduationProjectAPI.Utilities.General;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[Route("api")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}

		[Authorize(Roles = Roles.Mediator)]
		[HttpGet("[action]/[controller]")]
		public async Task<IActionResult> Mediators()
		{
			var userRegionId = await _context.Mediators
				.Where(m => m.Id == UserHandler.GetId(User))
				.Select(m => m.RegionId)
				.FirstAsync();

			if (userRegionId == null)
				return new BadRequest("Please complete your profile and choose your region");

			var query = _context.Cases.Take(5);

			var urgentCases = await query
				.Where(c => c.PriorityId == PriorityType.Urgent && c.StatusId == StatusType.Accepted)
				.SelectCaseElementDtoAsync();

			var myCases = await query
				.Where(c => c.MediatorId == UserHandler.GetId(User) && c.StatusId == StatusType.Accepted)
				.SelectCaseElementDtoAsync();

			var areaCases = await query
				.Where(c => c.RegionId == userRegionId && c.StatusId == StatusType.Accepted)
				.SelectCaseElementDtoAsync();

			var user = await _context.Mediators.SelectMediatorDetailsDtoAsync(UserHandler.GetId(User), await HttpContext.GetTokenAsync("Bearer", "access_token"));
			return new Success(new { urgentCases, myCases, areaCases, user });
		}

		[Authorize(Roles = Roles.Donator)]
		[HttpGet("[action]/[controller]")]
		public IActionResult Donators()
		{
			return new Success("Hi, I am donator");
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> FAQ()
		{
			return new Success(await _context.FAQs
				.Select(f => new { f.Title, f.Description })
				.ToArrayAsync());
		}
	}
}
