using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TestingController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public TestingController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> GetMediator(uint id)
		{
			var mediator = await _context.Mediators.AsNoTracking().Include(m => m.Status).FirstOrDefaultAsync(m => m.Id == id);
			return mediator == null ? NotFound() : Ok(mediator);
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> GetMediators()
		{
			var mediator = await _context.Mediators.AsNoTrackingWithIdentityResolution().Include(m => m.Status).ToArrayAsync();
			return new Success(mediator);
		}

		[HttpPost("[action]/{id}")]
		public async Task<IActionResult> AcceptMediator(uint id)
		{
			var mediator = await _context.Mediators.FirstAsync(m => m.Id == id);
			mediator.StatusId = await _context.Status.Where(s => s.Name == nameof(Status.Accepted)).Select(s => s.Id).FirstAsync();

			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> AcceptMediators()
		{
			var mediators = await _context.Mediators.ToArrayAsync();
			var acceptedStatusId = await _context.Status.Where(s => s.Name == nameof(Status.Accepted)).Select(s => s.Id).FirstAsync();
			foreach (var mediator in mediators)
				mediator.StatusId = acceptedStatusId;

			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> GetCase(uint id)
		{
			var @case = await _context.Cases.AsNoTracking().Include(m => m.Status).FirstOrDefaultAsync(m => m.Id == id);
			return @case == null ? NotFound() : Ok(@case);
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> GetCases()
		{
			var cases = await _context.Cases.AsNoTrackingWithIdentityResolution().Include(m => m.Status).ToArrayAsync();
			return new Success(cases);
		}
	}
}
