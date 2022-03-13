using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestingController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public TestingController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("GetMediator/{id}")]
		public async Task<IActionResult> GetMediator(uint id)
		{
			var mediator = await _context.Mediators.AsNoTracking().Include(m => m.Status).FirstOrDefaultAsync(m => m.Id == id);
			return mediator == null ? NotFound() : Ok(mediator);
		}

		[HttpGet("GetMediators")]
		public async Task<IActionResult> GetMediators()
		{
			var mediator = await _context.Mediators.AsNoTrackingWithIdentityResolution().Include(m => m.Status).ToArrayAsync();
			return new Success(mediator);
		}

		[HttpPost("AcceptMediator/{id}")]
		public async Task<IActionResult> AcceptMediator(uint id)
		{
			var mediator = await _context.Mediators.FirstAsync(m => m.Id == id);
			mediator.StatusId = await _context.Status.Where(s => s.Name == nameof(Status.Accepted)).Select(s => s.Id).FirstAsync();

			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpPost("AcceptMediators")]
		public async Task<IActionResult> AcceptMediators()
		{
			var mediators = await _context.Mediators.ToArrayAsync();
			var acceptedStatusId = await _context.Status.Where(s => s.Name == nameof(Status.Accepted)).Select(s => s.Id).FirstAsync();
			foreach (var mediator in mediators)
				mediator.StatusId = acceptedStatusId;

			await _context.SaveChangesAsync();
			return new Success();
		}
	}
}
