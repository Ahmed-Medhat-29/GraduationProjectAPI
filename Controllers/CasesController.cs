using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Case;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GraduationProjectAPI.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class CasesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly IMemoryCache _memoryCache;

		public CasesController(ApplicationDbContext context, IMapper mapper, IMemoryCache memoryCache)
		{
			_context = context;
			_mapper = mapper;
			_memoryCache = memoryCache;
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromForm] NewCaseDto dto)
		{
			var result = await ValidateCaseAsync(dto);
			if (result != null)
				return result;

			var newCase = _mapper.Map<Case>(dto);
			newCase.MediatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			newCase.StatusId = (byte)StatusType.Pending;
			await _context.AddAsync(newCase);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpGet("[action]/{id:min(1)}")]
		public async Task<IActionResult> Images(int id)
		{
			byte[] image;
			if (_memoryCache.TryGetValue(nameof(image) + id, out image))
				return File(image, "image/jpeg");

			image = await _context.Images
				.Where(m => m.Id == id)
				.Select(m => m.Data)
				.FirstOrDefaultAsync();

			if (image == null)
				return NotFound(null);

			_memoryCache.Set(nameof(image) + id, image, DateTimeOffset.Now.AddMinutes(1));
			return File(image, "image/jpeg");
		}

		// ********************** Private methods **********************************

		private async Task<BadRequest> ValidateCaseAsync(NewCaseDto dto)
		{
			var caseDb = await _context.Cases
				.Select(m => new Case
				{
					NationalId = m.NationalId,
					PhoneNumber = m.PhoneNumber
				}).FirstOrDefaultAsync(m => m.PhoneNumber == dto.PhoneNumber || m.NationalId == dto.NationalId);

			if (caseDb == null)
				return null;

			if (caseDb.PhoneNumber == dto.PhoneNumber)
				return new BadRequest(nameof(dto.PhoneNumber), "Phone number already exists");
			else
				return new BadRequest(nameof(dto.NationalId), "National id already exists");
		}
	}
}
