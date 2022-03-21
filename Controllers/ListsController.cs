using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
		private readonly IMapper _mapper;

		public ListsController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Governorates()
		{
			var governorates = await _context.Governorates.AsNoTracking().ToArrayAsync();
			return new Success(_mapper.Map<IEnumerable<GovernorateDto>>(governorates));
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> Cities(uint id)
		{
			var cities = await _context.Cities.AsNoTracking().Where(c => c.GovernorateId == id).ToArrayAsync();
			return new Success(_mapper.Map<IEnumerable<CityDto>>(cities));
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> Regions(uint id)
		{
			var regions = await _context.Regions.AsNoTracking().Where(r => r.CityId == id).ToArrayAsync();
			return new Success(_mapper.Map<IEnumerable<RegionDto>>(regions));
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
