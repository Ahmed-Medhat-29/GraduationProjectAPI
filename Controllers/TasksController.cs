using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Models.Reviews;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class TasksController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public TasksController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet("pending-mediators")]
		public async Task<IActionResult> PendingMediators([FromQuery] int page = 1)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var pendingMediators = await _context.Mediators
				.Where(m => m.StatusId == (byte)StatusType.Pending && !m.ReviewsAboutMe.Any(r => r.ReviewerId == userId))
				.OrderBy(m => m.DateRegistered)
				.Select(m => new
					{
						m.Id,
						m.Name,
						m.PhoneNumber,
						m.GeoLocation.Details,
						m.DateRegistered,
						ImageUrl = Paths.ProfilePicture(Request, m.Id)
					})
				.Skip(10 * (page - 1))
				.Take(10)
				.ToArrayAsync();

			return new Success(pendingMediators);
		}

		[HttpGet("pending-mediators/{id:min(1)}")]
		public async Task<IActionResult> PendingMediator(int id)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var mediator = await _context.Mediators
				.Where(m => m.Id == id && m.StatusId == (byte)StatusType.Pending && !m.ReviewsAboutMe.Any(r => r.ReviewerId == userId))
				.Select(m => new
				{
					Id = m.Id,
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					Address = m.Address,
					BirthDate = m.BirthDate,
					ImageUrl = Paths.ProfilePicture(Request, m.Id)
				})
				.FirstOrDefaultAsync();

			return new Success(mediator);
		}

		[HttpGet("pending-cases")]
		public async Task<IActionResult> PendingCases([FromQuery] int page = 1)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var pendingMediators = await _context.Cases
				.Where(m => m.StatusId == (byte)StatusType.Pending && m.MediatorId != userId && !m.CaseReviews.Any(r => r.MediatorId == userId))
				.OrderBy(m => m.DateRequested)
				.Select(m => new
				{
					m.Id,
					m.Title,
					m.NeededMoneyAmount,
					m.GeoLocation.Details,
					Age = (m.DateRequested - DateTime.Now).Days,
					ImageUrl = Paths.CaseImage(Request, m.Id)
				})
				.Skip(10 * (page - 1))
				.Take(10)
				.ToArrayAsync();

			return new Success(pendingMediators);
		}

		[HttpGet("pending-cases/{id:min(1)}")]
		public async Task<IActionResult> PendingCase(int id)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var mediator = await _context.Mediators
				.Where(m => m.Id == id && m.StatusId == (byte)StatusType.Pending && !m.ReviewsAboutMe.Any(r => r.ReviewerId == userId))
				.Select(m => new
				{
					Id = m.Id,
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					Address = m.Address,
					BirthDate = m.BirthDate,
					ImageUrl = Paths.ProfilePicture(Request, m.Id)
				})
				.FirstOrDefaultAsync();

			return new Success(mediator);
		}

		[HttpPost("review-mediator")]
		public async Task<IActionResult> ReviewMediator([FromForm] ReviewDto dto)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			if (dto.RevieweeId == userId)
				return new BadRequest("You can't review yurself");

			var isMediatorExists = await _context.Mediators.AnyAsync(m => m.Id == dto.RevieweeId && m.Status.Name == StatusType.Pending.ToString());
			if (!isMediatorExists)
				return new BadRequest("No pending mediator with such id found");

			var isReviewExists = await _context.MediatorReviews.AnyAsync(m => m.RevieweeId == dto.RevieweeId && m.ReviewerId == userId);
			if (isReviewExists)
				return new BadRequest("You have reviewed this mediator already");

			var review = _mapper.Map<MediatorReview>(dto);
			review.ReviewerId = userId;

			await _context.MediatorReviews.AddAsync(review);
			await _context.SaveChangesAsync();

			var reviewsCount = await _context.MediatorReviews.CountAsync(m => m.RevieweeId == dto.RevieweeId);
			if (reviewsCount >= 3)
			{
				var mediator = await _context.Mediators
					.Where(m => m.Id == dto.RevieweeId)
					.Select(m => new Mediator
					{
						Id = m.Id
					})
					.FirstOrDefaultAsync();

				_context.Mediators.Attach(mediator);
				mediator.StatusId = (byte)StatusType.Submitted;
				await _context.SaveChangesAsync();
			}

			return new Success();
		}

		[HttpPost("review-case")]
		public async Task<IActionResult> ReviewCase([FromForm] ReviewDto dto)
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
	}
}
