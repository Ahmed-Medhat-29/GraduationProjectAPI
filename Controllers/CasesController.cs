using System;
using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request.Cases;
using GraduationProjectAPI.DTOs.Response;
using GraduationProjectAPI.DTOs.Response.Cases;
using GraduationProjectAPI.DTOs.Response.Payments;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
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
	public class CasesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceScopeFactory _scopeFactory;

		public CasesController(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
		{
			_context = context;
			_scopeFactory = scopeFactory;
		}

		[HttpGet("urgent")]
		public async Task<IActionResult> UrgentCases(int page)
		{
			if (page <= 0) return NotFound(null);

			var urgentCasesCount = await _context.Cases.CountAsync(c => c.PriorityId == PriorityType.Urgent && c.StatusId == StatusType.Accepted);
			if (urgentCasesCount <= 0)
				return new SuccessWithPagination(Array.Empty<object>(), new Pagination(page));

			var urgentCases = await _context.Cases
				.Where(c => c.PriorityId == PriorityType.Urgent && c.StatusId == StatusType.Accepted)
				.OrderByDescending(c => c.DateRequested)
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.Select(c => new CaseElementDto
				{
					Id = c.Id,
					Name = c.Name,
					Title = c.Title,
					Priority = c.Priority.Name,
					Age = (short)(c.PaymentDate - DateTime.Now).TotalDays,
					FundRaised = c.CasePayments.Where(cp => cp.RoundNnumber == c.CurrentRound && cp.DateDelivered != null).Sum(cp => cp.Amount),
					TotalNeeded = c.NeededMoneyAmount,
					NumberOfContributer = c.CasePayments.Where(cp => cp.DateDelivered != null).Count(),
					ImageUrl = c.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				}).ToArrayAsync();

			return new SuccessWithPagination(urgentCases, new Pagination(page, urgentCasesCount, urgentCases.Length));
		}

		[HttpGet("mine")]
		public async Task<IActionResult> MyCases(int page, StatusType status)
		{
			if (page <= 0) return NotFound(null);

			var myCasesCount = await _context.Cases.CountAsync(c => c.MediatorId == UserHandler.GetId(User));
			if (myCasesCount <= 0)
				return new SuccessWithPagination(Array.Empty<object>(), new Pagination(page));

			var myCases = await _context.Cases
				.Where(c => c.MediatorId == UserHandler.GetId(User) && c.StatusId == status)
				.OrderByDescending(c => c.DateRequested)
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.Select(c => new CaseElementDto
				{
					Id = c.Id,
					Name = c.Name,
					Title = c.Title,
					Priority = c.Priority.Name,
					Age = (short)(c.PaymentDate - DateTime.Now).TotalDays,
					FundRaised = c.CasePayments.Where(cp => cp.RoundNnumber == c.CurrentRound && cp.DateDelivered != null).Sum(cp => cp.Amount),
					TotalNeeded = c.NeededMoneyAmount,
					NumberOfContributer = c.CasePayments.Where(cp => cp.DateDelivered != null).Count(),
					ImageUrl = c.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				}).ToArrayAsync();

			return new SuccessWithPagination(myCases, new Pagination(page, myCasesCount, myCases.Length));
		}

		[HttpGet("area")]
		public async Task<IActionResult> AreaCases(int page)
		{
			if (page <= 0) return NotFound(null);

			var userRegionId = await _context.Mediators
				.Where(m => m.Id == UserHandler.GetId(User))
				.Select(m => m.RegionId)
				.FirstOrDefaultAsync();

			if (userRegionId == null)
				return new BadRequest("Please complete your profile and choose your region");

			var areaCasesCount = await _context.Cases.CountAsync(c => c.StatusId == StatusType.Accepted && c.RegionId == userRegionId);
			if (areaCasesCount <= 0)
				return new SuccessWithPagination(Array.Empty<object>(), new Pagination(page));

			var areaCases = await _context.Cases
				.Where(c => c.StatusId == StatusType.Accepted && c.RegionId == userRegionId)
				.OrderByDescending(c => c.DateRequested)
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.Select(c => new CaseElementDto
				{
					Id = c.Id,
					Name = c.Name,
					Title = c.Title,
					Priority = c.Priority.Name,
					Age = (short)(c.PaymentDate - DateTime.Now).TotalDays,
					FundRaised = c.CasePayments.Where(cp => cp.RoundNnumber == c.CurrentRound && cp.DateDelivered != null).Sum(cp => cp.Amount),
					TotalNeeded = c.NeededMoneyAmount,
					NumberOfContributer = c.CasePayments.Where(cp => cp.DateDelivered != null).Count(),
					ImageUrl = c.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				}).ToArrayAsync();

			return new SuccessWithPagination(areaCases, new Pagination(page, areaCasesCount, areaCases.Length));
		}

		[HttpGet("{id:min(1)}")]
		public async Task<IActionResult> GetCase(int id)
		{
			var @case = await _context.Cases
				.Select(c => new CaseDetailsDto
				{
					Id = c.Id,
					Title = c.Title,
					Story = c.Story,
					Datetime = c.DateRequested,
					TotalNeeded = c.NeededMoneyAmount,
					Paid = c.CasePayments.Where(cp => cp.RoundNnumber == c.CurrentRound && cp.DateDelivered != null).Sum(cp => cp.Amount),
					Mediator = new CaseMediatorDto
					{
						Id = c.MediatorId,
						Name = c.Mediator.Name,
						ImageUrl = Paths.ProfilePicture(c.MediatorId)
					},
					History = c.CasePayments
						.Where(cp => cp.RoundNnumber == c.CurrentRound && cp.DateDelivered != null)
						.Select(cp => new PaymentElementDto
						{
							Name = cp.Mediator.Name,
							Amount = cp.Amount,
							RoundNumber = cp.RoundNnumber,
							Datetime = (DateTime)cp.DateDelivered,
							ImageUrl = cp.Case.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
						})
				})
				.FirstOrDefaultAsync(c => c.Id == id);

			if (@case == null)
				return NotFound(null);

			@case.ImagesUrls = await _context.Cases.Where(c => c.Id == @case.Id)
				.Select(c => c.Images.Select(i => Paths.CaseImage(i.Id)))
				.FirstOrDefaultAsync();

			return new Success(@case);
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromForm] NewCaseDto dto)
		{
			var newCase = dto.ToCase(UserHandler.GetId(User));
			await _context.AddAsync(newCase);
			await _context.SaveChangesAsync();
			_ = SendNotificationForNewCaseAsync(newCase);
			return new Success();
		}

		[HttpGet("[action]/{id:min(1)}")]
		public async Task<IActionResult> Images(int id)
		{
			var image = await _context.Images
				.Where(m => m.Id == id)
				.Select(m => m.Data)
				.FirstOrDefaultAsync();

			return image == null ? NotFound(null) : File(image, "image/jpeg");
		}

		// ************************ Private methods ************************

		private async Task SendNotificationForNewCaseAsync(Case newCase)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var mediatorsToBeNotified = await context.Mediators.Take(5)
						.Where(m => m.StatusId == StatusType.Accepted && m.Id != newCase.MediatorId)
						.OrderBy(m => m.GeoLocation.Location.Distance(newCase.GeoLocation.Location))
						.Select(m => new { m.Id, m.FirebaseToken })
						.ToArrayAsync();

			if (!mediatorsToBeNotified.Any())
				return;

			var notification = new Notification
			{
				Title = "New Case",
				Body = $"New case was added, <bold>{newCase.Title}</bold>",
				TaskId = newCase.Id,
				TypeId = Enums.NotificationType.Case,
				ImageUrl = newCase.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
			};

			var notificationDto = new NotificationDto(notification);
			var handler = new NotificationHandler(notificationDto);
			context.ChangeTracker.AutoDetectChangesEnabled = false;
			foreach (var mediator in mediatorsToBeNotified)
			{
				notification.Id = 0;
				notification.MediatorId = mediator.Id;
				await context.Notifications.AddAsync(notification);
				await context.SaveChangesAsync();
				await handler.SendAsync(mediator.FirebaseToken);
			}
		}
	}
}
