using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.ExtensionMethods;
using GraduationProjectAPI.Utilities.General;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraduationProjectAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = Roles.Mediator)]
	public class TasksController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceScopeFactory _scopeFactory;

		public TasksController(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
		{
			_context = context;
			_scopeFactory = scopeFactory;
		}

		[HttpGet("pending-mediators")]
		public async Task<IActionResult> PendingMediators(int page)
		{
			if (page <= 0) return NotFound(null);

			var pendingMediatorsCount = await _context.Mediators
				.Where(m => m.StatusId == StatusType.Pending && !m.ReviewsAboutMe.Any(r => r.ReviewerId == GetUserId()))
				.CountAsync();

			if (pendingMediatorsCount <= 0)
				return new SuccessWithPagination(Array.Empty<object>(), new Pagination(page));

			var pendingMediators = await _context.Mediators
				.Where(m => m.StatusId == StatusType.Pending && !m.ReviewsAboutMe.Any(r => r.ReviewerId == GetUserId()))
				.OrderBy(m => m.DateRegistered)
				.Select(m => new
				{
					Id = m.Id,
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					DateRegistered = m.DateRegistered,
					Details = m.GeoLocation.Details,
					ImageUrl = Paths.ProfilePicture(m.Id)
				})
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.ToArrayAsync();

			return new SuccessWithPagination(pendingMediators, new Pagination(page, pendingMediatorsCount, pendingMediators.Length));
		}

		[HttpGet("pending-mediators/{id:min(1)}")]
		public async Task<IActionResult> PendingMediator(int id)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var mediator = await _context.Mediators
				.Where(m => m.Id == id && m.StatusId == StatusType.Pending && !m.ReviewsAboutMe.Any(r => r.ReviewerId == userId))
				.Select(m => new
				{
					Id = m.Id,
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					BirthDate = m.BirthDate,
					ImageUrl = Paths.ProfilePicture(m.Id),
					Reviews = m.ReviewsAboutMe.Select(r => new
					{
						Name = r.Reviewer.Name,
						IsWorthy = r.IsWorthy,
						DateReviewed = r.DateReviewed,
						ImageUrl = Paths.ProfilePicture(r.ReviewerId)
					})
				})
				.FirstOrDefaultAsync();

			if (mediator == null)
				return NotFound(null);

			return new Success(mediator);
		}

		[HttpGet("pending-cases")]
		public async Task<object> PendingCases(int page)
		{
			if (page <= 0) return NotFound(null);

			var pendingCasesCount = await _context.Cases
				.Where(c => c.StatusId == StatusType.Pending && c.MediatorId != GetUserId() && !c.CaseReviews.Any(r => r.MediatorId == GetUserId()))
				.CountAsync();

			if (pendingCasesCount <= 0)
				return new SuccessWithPagination(Array.Empty<object>(), new Pagination(page));

			var pendingCases = await _context.Cases
				.Where(c => c.StatusId == StatusType.Pending && c.MediatorId != GetUserId() && !c.CaseReviews.Any(r => r.MediatorId == GetUserId()))
				.OrderBy(c => c.DateRequested)
				.Select(c => new
				{
					Id = c.Id,
					Title = c.Title,
					NeededMoneyAmount = c.NeededMoneyAmount,
					Age = (DateTime.Now - c.DateRequested).Days,
					Period = ((PeriodType)c.PeriodId).ToEnumString(),
					Details = c.GeoLocation.Details,
					ImageUrl = c.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				})
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.ToArrayAsync();

			return new SuccessWithPagination(pendingCases, new Pagination(page, pendingCasesCount, pendingCases.Length));
		}

		[HttpGet("pending-cases/{id:min(1)}")]
		public async Task<IActionResult> PendingCase(int id)
		{
			var @case = await _context.Cases
				.Where(c => c.Id == id && c.StatusId == StatusType.Pending && c.MediatorId != GetUserId() && !c.CaseReviews.Any(r => r.MediatorId == GetUserId()))
				.Select(c => new
				{
					Id = c.Id,
					Title = c.Title,
					NeededMoneyAmount = c.NeededMoneyAmount,
					DateRequested = c.DateRequested,
					Story = c.Story,
					Period = ((PeriodType)c.PeriodId).ToEnumString(),
					ImagesUrls = new List<string>(),
					Mediator = new
					{
						Id = c.MediatorId,
						Name = c.Mediator.Name,
						ImageUrl = Paths.ProfilePicture(c.MediatorId)
					},
					Reviews = c.CaseReviews.Select(r => new
					{
						Name = r.Mediator.Name,
						IsWorthy = r.IsWorthy,
						DateReviewed = r.DateReviewed,
						ImageUrl = Paths.ProfilePicture(r.MediatorId)
					})
				})
				.FirstOrDefaultAsync();

			if (@case == null)
				return NotFound(null);

			@case.ImagesUrls.AddRange(await _context.Cases
				.Where(c => c.Id == @case.Id)
				.Select(c => c.Images.Select(i => Paths.CaseImage(i.Id)))
				.FirstOrDefaultAsync());

			return new Success(@case);
		}

		[HttpPost("review-mediator")]
		public async Task<IActionResult> ReviewMediator([FromForm] NewReviewDto dto)
		{
			if (dto.RevieweeId == GetUserId())
				return new BadRequest("You can't review yourself");

			if (!await _context.Mediators.AnyAsync(m => m.Id == dto.RevieweeId && m.StatusId == StatusType.Pending))
				return new BadRequest("No pending mediator with such id found");

			if (await _context.MediatorReviews.AnyAsync(m => m.RevieweeId == dto.RevieweeId && m.ReviewerId == GetUserId()))
				return new BadRequest("You have reviewed this mediator already");

			await _context.MediatorReviews.AddAsync(dto.ToMediatorReview(GetUserId()));
			await _context.SaveChangesAsync();
			_ = CheckAndUpdateMediatorStatus(dto.RevieweeId);
			return new Success();
		}

		[HttpPost("review-case")]
		public async Task<IActionResult> ReviewCase([FromForm] NewReviewDto dto)
		{
			if (!await _context.Cases.AnyAsync(c => c.Id == dto.RevieweeId))
				return new BadRequest("Case was not found");

			if (await _context.CaseReviews.AnyAsync(c => c.CaseId == dto.RevieweeId && c.MediatorId == GetUserId()))
				return new BadRequest("Case has been reviewd already");

			await _context.CaseReviews.AddAsync(dto.ToCaseReview(GetUserId()));
			await _context.SaveChangesAsync();
			_ = CheckAndUpdateCaseStatus(dto.RevieweeId);
			return new Success();
		}

		private int GetUserId()
		{
			return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
		}

		private async Task CheckAndUpdateMediatorStatus(int id)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var isWorthyList = await context.MediatorReviews
				.Where(m => m.RevieweeId == id)
				.Select(m => m.IsWorthy)
				.ToArrayAsync();

			if (isWorthyList.Length < 3)
				return;

			var numberOfWorthy = isWorthyList.Count(worthy => worthy);
			var numberOfUnworthy = isWorthyList.Length - numberOfWorthy;
			if (numberOfWorthy < 3 && numberOfUnworthy < 3)
				return;

			var mediator = await context.Mediators
					.Select(m => new Mediator { Id = m.Id })
					.FirstOrDefaultAsync(m => m.Id == id);

			context.Mediators.Attach(mediator);

			if (numberOfWorthy >= 3)
				mediator.StatusId = StatusType.Submitted;
			else
				mediator.StatusId = StatusType.Rejected;

			await context.SaveChangesAsync();
		}

		private async Task CheckAndUpdateCaseStatus(int id)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var isWorthyList = await context.CaseReviews
				.Where(m => m.CaseId == id)
				.Select(m => m.IsWorthy)
				.ToArrayAsync();

			if (isWorthyList.Length < 3)
				return;

			var numberOfWorthy = isWorthyList.Count(worthy => worthy);
			var numberOfUnworthy = isWorthyList.Length - numberOfWorthy;
			if (numberOfWorthy < 3 && numberOfUnworthy < 3)
				return;

			var @case = await context.Cases
					.Select(m => new Case { Id = m.Id })
					.FirstOrDefaultAsync(m => m.Id == id);

			context.Cases.Attach(@case);

			if (numberOfWorthy >= 3)
				@case.StatusId = StatusType.Submitted;
			else
				@case.StatusId = StatusType.Rejected;

			await context.SaveChangesAsync();
		}
	}
}
