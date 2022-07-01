using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Response;
using GraduationProjectAPI.DTOs.Response.Cases;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Models.Shared;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace GraduationProjectAPI.Controllers
{
	[Route("api")]
	[ApiController]
	public class ListsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMemoryCache _memoryCache;
		private readonly IStringLocalizerFactory _localizerFactory;
		private readonly DateTimeOffset _cacheDuration;
		private readonly string _culture;

		public ListsController(ApplicationDbContext context, IMemoryCache memoryCache, IStringLocalizerFactory localizerFactory)
		{
			_context = context;
			_memoryCache = memoryCache;
			_localizerFactory = localizerFactory;
			_cacheDuration = DateTimeOffset.Now.AddMinutes(1);
			_culture = Thread.CurrentThread.CurrentCulture.Name;
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Governorates()
		{
			IEnumerable<ListItem> governorates;
			if (_memoryCache.TryGetValue(nameof(governorates) + _culture, out governorates))
				return new Success(governorates);

			governorates = await _context.Governorates
				.Select(g => new ListItem(g.Id, _culture == "ar" ? g.Name_AR : g.Name))
				.ToArrayAsync();

			_memoryCache.Set(nameof(governorates) + _culture, governorates, _cacheDuration);
			return new Success(governorates);
		}

		[HttpGet("[action]/{id:min(1)}")]
		public async Task<IActionResult> Cities(int id)
		{
			var cities = await _context.Cities
				.Where(c => c.GovernorateId == id)
				.Select(c => new ListItem(c.Id, _culture == "ar" ? c.Name_AR : c.Name))
				.ToArrayAsync();

			return new Success(cities);
		}

		[HttpGet("[action]/{id:min(1)}")]
		public async Task<IActionResult> Regions(int id)
		{
			var regions = await _context.Regions
				.Where(r => r.CityId == id)
				.Select(r => new ListItem(r.Id, _culture == "ar" ? r.Name_AR : r.Name))
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
			if (_memoryCache.TryGetValue(nameof(properties) + _culture, out properties))
				return new Success(properties);

			var categories = _context.Categories
				.Select(c => new Category
				{
					Id = c.Id,
					Name = _culture == "ar" ? c.Name_AR : c.Name
				}).ToArrayAsync();

			properties = new CaseProperties
			{
				Genders = GetGenders(),
				SocialStatus = GetSocialStatus()
			};
			
			var localizer = _localizerFactory.Create(typeof(RelationshipType));
			properties.Relationships = StaticValues.Relationships().Select(r => new Relationship { Id = r.Id, Name = localizer[r.Name].Value });
			
			var localizer1 = _localizerFactory.Create(typeof(PeriodType));
			properties.Periods = StaticValues.Periods().Select(p => new Period { Id = p.Id, Name = localizer1[p.Name].Value });

			var localizer2 = _localizerFactory.Create(typeof(PriorityType));
			properties.Priorities = StaticValues.Priorities().Select(p => new Priority { Id = p.Id, Name = localizer2[p.Name].Value });

			properties.Categories = await categories;

			_memoryCache.Set(nameof(properties) + _culture, properties, _cacheDuration);
			return new Success(properties);
		}

		[HttpGet("category-image/{id:min(1)}")]
		public async Task<IActionResult> CategoryImage(int id)
		{
			var image = await _context.Categories
				.Where(c => c.Id == id)
				.Select(c => c.Image)
				.FirstOrDefaultAsync();

			return image == null ? NotFound(null) : File(image, "image/jpeg");
		}

		[HttpGet("governorate-image/{id:min(1)}")]
		public async Task<IActionResult> GovernorateImage(int id)
		{
			var image = await _context.Governorates
				.Where(c => c.Id == id)
				.Select(c => c.Image)
				.FirstOrDefaultAsync();

			return image == null ? NotFound(null) : File(image, "image/jpeg");
		}

		private IEnumerable<Gender> GetGenders()
		{
			IEnumerable<Gender> genders;
			if (_memoryCache.TryGetValue(nameof(genders) + _culture, out genders))
				return genders;

			var localizer = _localizerFactory.Create(typeof(GenderType));
			genders = StaticValues.Genders().Select(g => new Gender { Id = g.Id, Name = localizer[g.Name].Value });
			_memoryCache.Set(nameof(genders) + _culture, genders, _cacheDuration);
			return genders;
		}

		private IEnumerable<SocialStatus> GetSocialStatus()
		{
			IEnumerable<SocialStatus> socialStatus;
			if (_memoryCache.TryGetValue(nameof(socialStatus) + _culture, out socialStatus))
				return socialStatus;

			var localizer = _localizerFactory.Create(typeof(SocialStatusType));
			socialStatus = StaticValues.SocialStatus().Select(s => new SocialStatus { Id = s.Id, Name = localizer[s.Name].Value });
			_memoryCache.Set(nameof(socialStatus), socialStatus + _culture, _cacheDuration);
			return socialStatus;
		}
	}
}
