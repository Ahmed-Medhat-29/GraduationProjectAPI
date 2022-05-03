using System;
using System.Device.Location;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.DTOs.Case;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.General;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace GraduationProjectAPI.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class CasesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMemoryCache _memoryCache;
		private readonly IServiceScopeFactory _scopeFactory;

		public CasesController(ApplicationDbContext context, IMemoryCache memoryCache, IServiceScopeFactory scopeFactory)
		{
			_context = context;
			_memoryCache = memoryCache;
			_scopeFactory = scopeFactory;
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromForm] NewCaseDto dto)
		{
			var newCase = dto.ToCase(GetUserId());
			await _context.AddAsync(newCase);
			await _context.SaveChangesAsync();
			_ = SendNotificationForNewCaseAsync(newCase);
			return new Success();
		}

		[HttpGet("[action]/{id:min(1)}")]
		public async Task<IActionResult> Images(int id)
		{
			byte[] caseImage;
			if (_memoryCache.TryGetValue(nameof(caseImage) + id, out caseImage))
				return File(caseImage, "image/jpeg");

			caseImage = await _context.Images
				.Where(m => m.Id == id)
				.Select(m => m.Data)
				.FirstOrDefaultAsync();

			if (caseImage == null)
				return NotFound(null);

			_memoryCache.Set(nameof(caseImage) + id, caseImage, DateTimeOffset.Now.AddMinutes(10));
			return File(caseImage, "image/jpeg");
		}

		// ********************** Private methods **********************************

		private async Task SendNotificationForNewCaseAsync(Case newCase)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var geoLocations = await context.Mediators
						.Where(m => m.StatusId == (byte)StatusType.Accepted && m.Id != newCase.MediatorId)
						.Select(m => new
						{
							Id = m.GeoLocationId,
							GeoCoordinate = new GeoCoordinate(m.GeoLocation.Latitude, m.GeoLocation.Longitude)
						})
						.ToArrayAsync();

			var caseCoordinate = new GeoCoordinate(newCase.GeoLocation.Latitude, newCase.GeoLocation.Longitude);

			var closestLocationsId = geoLocations
				.OrderBy(l => caseCoordinate.GetDistanceTo(l.GeoCoordinate))
				.Select(l => l.Id)
				.Take(5);

			var mediatorsToBeNotified = await context.Mediators
				.Where(m => closestLocationsId.Contains(m.GeoLocationId))
				.Select(m => new { m.Id, m.FirebaseToken })
				.ToArrayAsync();

			if (!mediatorsToBeNotified.Any())
				return;

			var notification = new Notification
			{
				Title = "New Case",
				Body = $"New case was added, <bold>{newCase.Title}</bold>",
				TaskId = newCase.Id,
				TypeId = (byte)Enums.NotificationType.Case,
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

		private int GetUserId()
		{
			return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
		}
	}
}
