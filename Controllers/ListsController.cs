using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[Route("api")]
	[ApiController]
	public class ListsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ListsController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Governorates()
		{
			return new Success(await _context.Governorates
				.Select(g => new ListItem
				{
					Id = g.Id,
					Name = g.Name
				}).ToArrayAsync());
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> Cities(uint id)
		{
			return new Success(await _context.Cities
				.Where(c => c.GovernorateId == id)
				.Select(g => new ListItem
				{
					Id = g.Id,
					Name = g.Name
				}).ToArrayAsync());
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> Regions(uint id)
		{
			return new Success(await _context.Regions
				.Where(r => r.CityId == id)
				.Select(g => new ListItem
				{
					Id = g.Id,
					Name = g.Name
				}).ToArrayAsync());
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Genders()
		{
			return new Success(await _context.Genders.AsNoTracking().ToArrayAsync());
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> SocialStatus()
		{
			return new Success(await _context.SocialStatus.AsNoTracking().ToArrayAsync());
		}
	}
}
