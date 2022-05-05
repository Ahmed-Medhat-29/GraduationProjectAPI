using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Utilities.CustomApiResponses;
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

		[HttpGet("[action]/{id:min(1)}")]
		public async Task<IActionResult> GetMediator(int id)
		{
			var mediator = await _context.Mediators.AsNoTracking()
				.Include(m => m.Status)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (mediator == null)
				return new BadRequest($"Mediator with id: {id} could not be found");

			return new Success(mediator);
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> GetMediators()
		{
			return new Success(await _context.Mediators.AsNoTrackingWithIdentityResolution()
				.Include(m => m.Status)
				.ToArrayAsync());
		}

		[HttpPost("[action]/{id:min(1)}")]
		public async Task<IActionResult> AcceptMediator(int id)
		{
			var mediator = await _context.Mediators.FirstOrDefaultAsync(m => m.Id == id);
			if (mediator == null)
				return NotFound($"Mediator with id: {id} could not be found");

			mediator.StatusId = StatusType.Accepted;
			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> AcceptMediators()
		{
			var mediators = await _context.Mediators
				.Where(m => m.StatusId == StatusType.Pending || m.StatusId == StatusType.Submitted)
				.ToArrayAsync();

			if (!mediators.Any())
				return new Success();

			foreach (var mediator in mediators)
				mediator.StatusId = StatusType.Accepted;

			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpGet("[action]/{id:min(1)}")]
		public async Task<IActionResult> GetCase(int id)
		{
			var @case = await _context.Cases.AsNoTracking()
				.Include(m => m.Status)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (@case == null)
				return NotFound($"Case with id: {id} could not be found");

			return new Success(@case);
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> GetCases()
		{
			return new Success(await _context.Cases.AsNoTrackingWithIdentityResolution()
				.Include(m => m.Status)
				.ToArrayAsync());
		}

		[HttpPost("[action]/{id:min(1)}")]
		public async Task<IActionResult> AcceptCase(int id)
		{
			var @case = await _context.Cases.FirstOrDefaultAsync(m => m.Id == id);
			if (@case == null)
				return NotFound($"Case with id: {id} could not be found");

			@case.StatusId = StatusType.Accepted;
			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> AcceptCases()
		{
			var cases = await _context.Cases
				.Where(m => m.StatusId == StatusType.Pending || m.StatusId == StatusType.Submitted)
				.ToArrayAsync();

			if (!cases.Any())
				return new Success();

			foreach (var @case in cases)
				@case.StatusId = StatusType.Accepted;

			await _context.SaveChangesAsync();
			return new Success();
		}
	}
}
