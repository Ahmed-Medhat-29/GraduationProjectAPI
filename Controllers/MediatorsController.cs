using System;
using System.Device.Location;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.DTOs.Case;
using GraduationProjectAPI.DTOs.Chat;
using GraduationProjectAPI.DTOs.Mediator;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
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
	public class MediatorsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMemoryCache _memoryCache;
		private readonly IServiceScopeFactory _scopeFactory;

		public MediatorsController(ApplicationDbContext context, IMemoryCache memoryCache, IServiceScopeFactory scopeFactory)
		{
			_context = context;
			_memoryCache = memoryCache;
			_scopeFactory = scopeFactory;
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Home()
		{
			var cases = await _context.Cases
				.Where(c => c.StatusId == (byte)StatusType.Accepted)
				.Select(c => new CaseElementDto
				{
					Id = c.Id,
					Name = c.Name,
					Title = c.Title,
					Priority = c.Priority.Name,
					Age = ((short)(DateTime.Now - c.DateRequested).TotalDays),
					FundRaised = 4000,
				}).ToArrayAsync();

			var images = await _context.Images
				.Where(i => cases.Select(c => c.Id).Contains(i.CaseId))
				.Select(i => new Image
				{
					Id = i.Id,
					CaseId = i.CaseId
				})
				.ToArrayAsync();

			foreach (var @case in cases)
				@case.ImagesUrl = images.Where(i => i.CaseId == @case.Id).Select(i => Paths.CaseImage(i.Id)).ToArray();

			return new Success(cases);
		}

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> Register([FromForm] RegisterDto dto)
		{
			var mediator = dto.ToMediator();
			await _context.Mediators.AddAsync(mediator);
			await _context.SaveChangesAsync();
			_ = SendNotificationForNewMediatorAsync(mediator);
			return new Success();
		}

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> SignIn([FromForm] SignInRequestDto dto, [FromServices] IAuthenticationTokenGenerator tokenGenerator)
		{
			await UpdateFirebaseTokenAsync(dto.PhoneNumber, dto.FirebaseToken);

			return new Success(await _context.Mediators
				.Select(m => new
				{
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					NationalId = m.NationalId,
					Job = m.Job,
					Address = m.Address,
					BirthDate = m.BirthDate,
					Bio = m.Bio,
					Completed = m.Completed,
					JwtToken = tokenGenerator.Generate(m.Id.ToString()),
					FirebaseToken = m.FirebaseToken,
					Gender = ((GenderType)m.GenderId).ToString(),
					Region = m.Region.Name,
					SocialStatus = ((SocialStatusType)m.SocialStatusId).ToString(),
					Locale = ((LocaleType)m.LocaleId).ToString(),
					Status = ((StatusType)m.StatusId).ToString(),
					ProfileImageUrl = Paths.ProfilePicture(m.Id),
					NationalIdImageUrl = Paths.NationalIdImage(m.Id),
					GeoLocation = new
					{
						Longitude = m.GeoLocation.Longitude,
						Latitude = m.GeoLocation.Latitude,
						Details = m.GeoLocation.Details
					}
				})
				.FirstAsync(m => m.PhoneNumber == dto.PhoneNumber));
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Profile()
		{
			return new Success(await _context.Mediators
				.Where(m => m.Id == GetUserId())
				.Select(m => new
				{
					Name = m.Name,
					Bio = m.Bio,
					SocialStatusId = m.SocialStatusId,
					ImageUrl = Paths.ProfilePicture(m.Id),
				})
				.FirstAsync());
		}

		[HttpPatch("[action]")]
		public async Task<IActionResult> Profile([FromForm] CompleteProfileDto dto)
		{
			var mediator = new Mediator { Id = GetUserId() };
			_context.Mediators.Attach(mediator);
			dto.UpdateMediator(mediator);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpGet("profile-image/{id:min(1)}")]
		public async Task<IActionResult> ProfileImage(int id)
		{
			byte[] profileImage;
			if (_memoryCache.TryGetValue(nameof(profileImage) + id, out profileImage))
				return File(profileImage, "image/jpeg");

			profileImage = await _context.Mediators
				.Where(m => m.Id == id)
				.Select(m => m.ProfileImage)
				.FirstOrDefaultAsync();

			if (profileImage == null)
				return NotFound(null);

			_memoryCache.Set(nameof(profileImage) + id, profileImage, DateTimeOffset.Now.AddMinutes(10));
			return File(profileImage, "image/jpeg");
		}

		[HttpGet("nationalid-image/{id:min(1)}")]
		public async Task<IActionResult> NationalIdImage(int id)
		{
			var image = await _context.Mediators
				.Where(m => m.Id == id)
				.Select(m => m.NationalIdImage)
				.FirstOrDefaultAsync();

			return image == null ? NotFound(null) : File(image, "image/jpeg");
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Notifications(int page)
		{
			if (page <= 0) return NotFound(null);

			var notificationsCount = await _context.Notifications
				.CountAsync(n => n.MediatorId == GetUserId());

			if (notificationsCount <= 0)
				return new SuccessWithPagination(Array.Empty<NotificationDto>(), new Pagination(page));

			var notifications = await _context.Notifications
				.Where(n => n.MediatorId == GetUserId())
				.OrderBy(n => n.DateTime)
				.Select(n => new
				{
					Id = n.Id,
					Title = n.Title,
					Body = n.Body,
					IsRead = n.IsRead,
					TaskId = n.TaskId,
					DateTime = n.DateTime,
					ImageUrl = n.ImageUrl,
					Type = ((Enums.NotificationType)n.TypeId).ToString()
				})
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.ToArrayAsync();

			var pagination = new Pagination(page, notificationsCount, notifications.Length);
			if (notifications.Any(n => !n.IsRead))
				_ = SetNotificationsAsReadAsync(GetUserId());

			return new SuccessWithPagination(notifications, pagination);
		}

		[AllowAnonymous]
		[HttpGet("[action]")]
		public async Task<IActionResult> FAQ()
		{
			return new Success(await _context.FAQs
				.Select(f => new { f.Title, f.Description })
				.ToArrayAsync());
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Message([FromForm] ChatDto dto)
		{
			var chat = dto.ToChat(GetUserId(), Enums.MessageType.Sent);
			if (chat.ChatId == 0)
				chat.ChatId = await _context.Chats.MaxAsync(c => c.ChatId) + 1;

			await _context.Chats.AddAsync(chat);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> ValidateNumber([FromForm] PhoneNumberDto numberDTO)
		{
			var isNumberExists = await _context.Mediators.AnyAsync(m => m.PhoneNumber == numberDTO.PhoneNumber);
			return isNumberExists ? new BadRequest("Number is registered already") : new Success();
		}

		// ********************** Private methods **********************************

		private async Task SendNotificationForNewMediatorAsync(Mediator newMediator)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var geoLocations = await context.Mediators
						.Where(m => m.StatusId == (byte)StatusType.Accepted)
						.Select(m => new
						{
							Id = m.GeoLocationId,
							GeoCoordinate = new GeoCoordinate(m.GeoLocation.Latitude, m.GeoLocation.Longitude)
						})
						.ToArrayAsync();

			var mediatorCoordinate = new GeoCoordinate(newMediator.GeoLocation.Latitude, newMediator.GeoLocation.Longitude);

			var closestLocationsId = geoLocations
				.OrderBy(l => mediatorCoordinate.GetDistanceTo(l.GeoCoordinate))
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
				Title = "New Mediator",
				Body = $"<bold>{newMediator.Name}</bold> has registered, Check him out!!!",
				TaskId = newMediator.Id,
				TypeId = (byte)Enums.NotificationType.Mediator,
				ImageUrl = Paths.ProfilePicture(newMediator.Id)
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

		private async Task SetNotificationsAsReadAsync(int mediatorId)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			await context.Database.ExecuteSqlRawAsync($"UPDATE [{nameof(context.Notifications)}] SET [{nameof(Notification.IsRead)}] = 1 WHERE [{nameof(Notification.MediatorId)}] = {mediatorId} AND [{nameof(Notification.IsRead)}] = 0;");
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Notify([FromForm] string token, [FromForm] string title, [FromForm] string body)
		{
			var notification = new Notification
			{
				Title = title,
				Body = body
			};

			var notificationDto = new NotificationDto(notification);
			var handler = new NotificationHandler(notificationDto);
			await handler.SendAsync(token);
			return new Success();
		}

		private int GetUserId()
		{
			return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
		}

		private async Task UpdateFirebaseTokenAsync(string phoneNumber, string token)
		{
			var mediator = await _context.Mediators
				.Where(m => m.PhoneNumber == phoneNumber && m.FirebaseToken != token)
				.Select(m => new Mediator { Id = m.Id })
				.FirstOrDefaultAsync();

			if (mediator != null)
			{
				_context.Mediators.Attach(mediator);
				mediator.FirebaseToken = token;
				await _context.SaveChangesAsync();
			}
		}
	}
}
