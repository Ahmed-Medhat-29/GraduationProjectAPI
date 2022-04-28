using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.DTOs.Case;
using GraduationProjectAPI.Models.Shared;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GraduationProjectAPI.Controllers
{
	[Route("api")]
	[ApiController]
	public class ListsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMemoryCache _memoryCache;

		public ListsController(ApplicationDbContext context, IMemoryCache memoryCache)
		{
			_context = context;
			_memoryCache = memoryCache;
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Governorates()
		{
			IEnumerable<ListItem> governorates;
			if (_memoryCache.TryGetValue(nameof(governorates), out governorates))
				return new Success(governorates);

			governorates = await _context.Governorates
				.Select(g => new ListItem(g.Id, g.Name))
				.ToArrayAsync();

			_memoryCache.Set(nameof(governorates), governorates, DateTimeOffset.Now.AddMinutes(1));
			return new Success(governorates);
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> Cities(uint id)
		{
			IEnumerable<ListItem> cities;
			if (_memoryCache.TryGetValue(nameof(cities) + id, out cities))
				return new Success(cities);

			cities = await _context.Cities
				.Where(c => c.GovernorateId == id)
				.Select(c => new ListItem(c.Id, c.Name))
				.ToArrayAsync();

			_memoryCache.Set(nameof(cities) + id, cities, DateTimeOffset.Now.AddMinutes(1));
			return new Success(cities);
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> Regions(uint id)
		{
			IEnumerable<ListItem> regions;
			if (_memoryCache.TryGetValue(nameof(regions) + id, out regions))
				return new Success(regions);

			regions = await _context.Regions
				.Where(r => r.CityId == id)
				.Select(r => new ListItem(r.Id, r.Name))
				.ToArrayAsync();

			_memoryCache.Set(nameof(regions) + id, regions, DateTimeOffset.Now.AddMinutes(1));
			return new Success(regions);
		}

		[HttpGet("[action]")]
		public IActionResult Genders()
		{
			return new Success(GetGenders());
		}

		[HttpGet("[action]")]
		public IActionResult SocialStatus()
		{
			return new Success(GetSocialStatus());
		}

		[HttpGet("case-properties")]
		public async Task<IActionResult> CaseProperties()
		{
			CaseProperties properties;
			if (_memoryCache.TryGetValue(nameof(properties), out properties))
				return new Success(properties);

			var categories = _context.Categories.AsNoTracking().ToArrayAsync();
			properties = new CaseProperties
			{
				Genders = GetGenders(),
				SocialStatus = GetSocialStatus(),
				Relationships = StaticValues.Relationships(),
				Periods = StaticValues.Periods(),
				Priorities = StaticValues.Priorities(),
				Categories = await categories,
			};

			_memoryCache.Set(nameof(properties), properties, DateTimeOffset.Now.AddMinutes(1));
			return new Success(properties);
		}

		private IEnumerable<Gender> GetGenders()
		{
			IEnumerable<Gender> genders;
			if (_memoryCache.TryGetValue(nameof(genders), out genders))
				return genders;

			genders = StaticValues.Genders();
			_memoryCache.Set(nameof(genders), genders, DateTimeOffset.Now.AddMinutes(1));
			return genders;
		}

		private IEnumerable<SocialStatus> GetSocialStatus()
		{
			IEnumerable<SocialStatus> socialStatus;
			if (_memoryCache.TryGetValue(nameof(socialStatus), out socialStatus))
				return socialStatus;

			socialStatus = StaticValues.SocialStatus();
			_memoryCache.Set(nameof(socialStatus), socialStatus, DateTimeOffset.Now.AddMinutes(1));
			return socialStatus;
		}
	}
}
