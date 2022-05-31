using System;
using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request;
using GraduationProjectAPI.DTOs.Response;
using GraduationProjectAPI.DTOs.Response.Cases;
using GraduationProjectAPI.DTOs.Response.Mediators;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities;
using GraduationProjectAPI.Utilities.CustomApiResponses;
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
				.CountAsync(m => m.StatusId == StatusType.Pending && !m.ReviewsAboutMe.Any(r => r.ReviewerId == UserHandler.GetId(User)));

			if (pendingMediatorsCount <= 0)
				return new SuccessWithPagination(Array.Empty<object>(), new Pagination(page));

			var pendingMediators = await _context.Mediators
				.Where(m => m.StatusId == StatusType.Pending && !m.ReviewsAboutMe.Any(r => r.ReviewerId == UserHandler.GetId(User)))
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.OrderBy(m => m.DateRegistered)
				.Select(m => new MediatorTaskElementDto
				{
					Id = m.Id,
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					DateRegistered = m.DateRegistered,
					Details = m.GeoLocation.Details,
					ImageUrl = Paths.ProfilePicture(m.Id)
				}).ToArrayAsync();

			return new SuccessWithPagination(pendingMediators, new Pagination(page, pendingMediatorsCount, pendingMediators.Length));
		}

		[HttpGet("pending-mediators/{id:min(1)}")]
		public async Task<IActionResult> PendingMediator(int id)
		{
			var mediator = await _context.Mediators
				.Where(m => m.Id == id && m.StatusId == StatusType.Pending &&
							!m.ReviewsAboutMe.Any(r =>
								r.ReviewerId == UserHandler.GetId(User)))
				.Select(m => new MediatorTaskDetailsDto
				{
					Id = m.Id,
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					BirthDate = m.BirthDate,
					ImageUrl = Paths.ProfilePicture(m.Id),
					Reviews = m.ReviewsAboutMe.Select(r => new ReviewDto
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
				.CountAsync(c => c.StatusId == StatusType.Pending && c.MediatorId != UserHandler.GetId(User) && !c.CaseReviews.Any(r => r.MediatorId == UserHandler.GetId(User)));

			if (pendingCasesCount <= 0)
				return new SuccessWithPagination(Array.Empty<object>(), new Pagination(page));

			var pendingCases = await _context.Cases
				.Where(c => c.StatusId == StatusType.Pending && c.MediatorId != UserHandler.GetId(User) &&
							!c.CaseReviews.Any(r => r.MediatorId == UserHandler.GetId(User)))
				.OrderBy(c => c.DateRequested)
				.Select(c => new ReviewCaseTaskElementDto
				{
					Id = c.Id,
					Title = c.Title,
					NeededMoneyAmount = c.NeededMoneyAmount,
					Age = (short)(DateTime.Now - c.DateRequested).Days,
					Period = c.PeriodId.ToEnumString(),
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
				.Where(c => c.Id == id &&
							c.StatusId == StatusType.Pending &&
							c.MediatorId != UserHandler.GetId(User) &&
							!c.CaseReviews.Any(r => r.MediatorId == UserHandler.GetId(User)))
				.Select(c => new ReviewCaseTaskDetailsDto
				{
					Id = c.Id,
					Title = c.Title,
					NeededMoneyAmount = c.NeededMoneyAmount,
					DateRequested = c.DateRequested,
					Story = c.Story,
					Period = c.PeriodId.ToEnumString(),
					Mediator = new CaseMediatorDto
					{
						Id = c.MediatorId,
						Name = c.Mediator.Name,
						ImageUrl = Paths.ProfilePicture(c.MediatorId)
					},
					Reviews = c.CaseReviews.Select(r => new DTOs.Response.ReviewDto
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

			@case.ImagesUrls = await _context.Cases
				.Where(c => c.Id == @case.Id)
				.Select(c => c.Images.Select(i => Paths.CaseImage(i.Id)))
				.FirstOrDefaultAsync();

			return new Success(@case);
		}

		[HttpPost("review-mediator")]
		public async Task<IActionResult> ReviewMediator([FromForm] NewReviewDto dto)
		{
			if (dto.RevieweeId == UserHandler.GetId(User))
				return new BadRequest("You can't review yourself");

			if (!await _context.Mediators.AnyAsync(m => m.Id == dto.RevieweeId && m.StatusId == StatusType.Pending))
				return new BadRequest("No pending mediator with such id found");

			if (await _context.MediatorReviews.AnyAsync(m => m.RevieweeId == dto.RevieweeId && m.ReviewerId == UserHandler.GetId(User)))
				return new BadRequest("You have reviewed this mediator already");

			await _context.MediatorReviews.AddAsync(dto.ToMediatorReview(UserHandler.GetId(User)));
			await _context.SaveChangesAsync();
			_ = CheckAndUpdateMediatorStatus(dto.RevieweeId);
			return new Success();
		}

		[HttpPost("review-case")]
		public async Task<IActionResult> ReviewCase([FromForm] NewReviewDto dto)
		{
			if (!await _context.Cases.AnyAsync(c => c.Id == dto.RevieweeId))
				return new BadRequest("Case was not found");

			if (await _context.CaseReviews.AnyAsync(c => c.CaseId == dto.RevieweeId && c.MediatorId == UserHandler.GetId(User)))
				return new BadRequest("Case has been reviewd already");

			await _context.CaseReviews.AddAsync(dto.ToCaseReview(UserHandler.GetId(User)));
			await _context.SaveChangesAsync();
			_ = CheckAndUpdateCaseStatus(dto.RevieweeId);
			return new Success();
		}

		// ************************ Private methods ************************

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
