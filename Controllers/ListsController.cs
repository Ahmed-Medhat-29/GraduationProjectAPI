using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Response;
using GraduationProjectAPI.DTOs.Response.Cases;
using GraduationProjectAPI.Models.CaseProperties;
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
		private readonly DateTimeOffset _cacheDuration;

		public ListsController(ApplicationDbContext context, IMemoryCache memoryCache)
		{
			_context = context;
			_memoryCache = memoryCache;
			_cacheDuration = DateTimeOffset.Now.AddMinutes(1);
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

			_memoryCache.Set(nameof(governorates), governorates, _cacheDuration);
			return new Success(governorates);
		}

		[HttpGet("[action]/{id:min(1)}")]
		public async Task<IActionResult> Cities(int id)
		{
			var cities = await _context.Cities
				.Where(c => c.GovernorateId == id)
				.Select(c => new ListItem(c.Id, c.Name))
				.ToArrayAsync();

			return new Success(cities);
		}

		[HttpGet("[action]/{id:min(1)}")]
		public async Task<IActionResult> Regions(int id)
		{
			var regions = await _context.Regions
				.Where(r => r.CityId == id)
				.Select(r => new ListItem(r.Id, r.Name))
				.ToArrayAsync();

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

			var categories = _context.Categories
				.Select(c => new Category
				{
					Id = c.Id,
					Name = c.Name
				}).ToArrayAsync();

			properties = new CaseProperties
			{
				Genders = GetGenders(),
				SocialStatus = GetSocialStatus(),
				Relationships = StaticValues.Relationships(),
				Periods = StaticValues.Periods(),
				Priorities = StaticValues.Priorities(),
				Categories = await categories,
			};

			_memoryCache.Set(nameof(properties), properties, _cacheDuration);
			return new Success(properties);
		}

		private IEnumerable<Gender> GetGenders()
		{
			IEnumerable<Gender> genders;
			if (_memoryCache.TryGetValue(nameof(genders), out genders))
				return genders;

			genders = StaticValues.Genders();
			_memoryCache.Set(nameof(genders), genders, _cacheDuration);
			return genders;
		}

		private IEnumerable<SocialStatus> GetSocialStatus()
		{
			IEnumerable<SocialStatus> socialStatus;
			if (_memoryCache.TryGetValue(nameof(socialStatus), out socialStatus))
				return socialStatus;

			socialStatus = StaticValues.SocialStatus();
			_memoryCache.Set(nameof(socialStatus), socialStatus, _cacheDuration);
			return socialStatus;
		}
	}
}
