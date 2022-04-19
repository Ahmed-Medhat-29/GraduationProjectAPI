﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.DTOs.Case;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Models.Reviews;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CasesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public CasesController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[Authorize]
		[HttpGet("[action]/{id:min(1)}")]
		public async Task<IActionResult> Image(int id)
		{
			var image = await _context.Images
				.Where(i => i.Id == id)
				.Select(m => m.Data)
				.FirstOrDefaultAsync();

			return image == null ? NotFound(null) : File(image, "image/jpeg");
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Add([FromForm] NewCaseDto dto)
		{
			var result = await IsCasesAddedAsync(dto);
			if (result != null)
				return result;

			var pendingStatusId = _context.Status
				.Where(s => s.Name == StatusType.Pending.ToString())
				.Select(s => s.Id)
				.FirstAsync();

			var newCase = _mapper.Map<Case>(dto);
			newCase.MediatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			newCase.StatusId = await pendingStatusId;
			await _context.AddAsync(newCase);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[Authorize]
		[HttpPost("[action]")]
		public async Task<IActionResult> Review([FromForm] ReviewDto dto)
		{
			if (!await _context.Cases.AnyAsync(c => c.Id == dto.RevieweeId))
				return new BadRequest("Case was not found");

			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			if (await _context.CaseReviews.AnyAsync(c => c.CaseId == dto.RevieweeId && c.MediatorId == userId))
				return new BadRequest("Case has been reviewd already");

			var review = _mapper.Map(dto, new CaseReview(userId));
			await _context.CaseReviews.AddAsync(review);
			await _context.SaveChangesAsync();
			return new Success();
		}

		private async Task<BadRequest> IsCasesAddedAsync(NewCaseDto dto)
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
