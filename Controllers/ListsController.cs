using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.Customs.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[Route("api")]
	[ApiController]
	[AllowAnonymous]
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
		[ProducesResponseType(typeof(Governorate[]), StatusCodes.Status200OK)]
		public async Task<IActionResult> Governorates()
		{
			var governorates = await _context.Governorates.AsNoTracking().ToArrayAsync();
			return new Success(_mapper.Map<IEnumerable<GovernorateDTO>>(governorates));
		}

		[HttpGet("[action]/{id}")]
		[ProducesResponseType(typeof(City[]), StatusCodes.Status200OK)]
		public async Task<IActionResult> Cities(uint id)
		{
			var cities = await _context.Cities.AsNoTracking().Where(c => c.GovernorateId == id).ToArrayAsync();
			return new Success(_mapper.Map<IEnumerable<CityDTO>>(cities));
		}

		[HttpGet("[action]/{id}")]
		[ProducesResponseType(typeof(Region[]), StatusCodes.Status200OK)]
		public async Task<IActionResult> Regions(uint id)
		{
			var regions = await _context.Regions.AsNoTracking().Where(r => r.CityId == id).ToArrayAsync();
			return new Success(_mapper.Map<IEnumerable<RegionDTO>>(regions));
		}

		[HttpGet("[action]")]
		[ProducesResponseType(typeof(Gender[]), StatusCodes.Status200OK)]
		public async Task<IActionResult> Genders()
		{
			return new Success(await _context.Genders.AsNoTracking().ToArrayAsync());
		}

		[HttpGet("[action]")]
		[ProducesResponseType(typeof(SocialStatus[]), StatusCodes.Status200OK)]
		public async Task<IActionResult> SocialStatus()
		{
			return new Success(await _context.SocialStatus.AsNoTracking().ToArrayAsync());
		}
	}
}
