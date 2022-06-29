using System;
using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Common;
using GraduationProjectAPI.DTOs.Request;
using GraduationProjectAPI.DTOs.Request.Mediators;
using GraduationProjectAPI.DTOs.Response;
using GraduationProjectAPI.DTOs.Response.Mediators;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
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
	public class MediatorsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceScopeFactory _scopeFactory;

		public MediatorsController(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
		{
			_context = context;
			_scopeFactory = scopeFactory;
		}

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> Register([FromForm] RegisterDto dto)
		{
			var mediator = dto.ToMediator();
			await _context.Mediators.AddAsync(mediator);
			await _context.SaveChangesAsync();
			_ = NewMediatorNotificationAsync(mediator);
			return new Success();
		}

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> SignIn([FromForm] SignInRequestDto dto, [FromServices] IAuthenticationTokenGenerator tokenGenerator)
		{
			var responseDto = await _context.Mediators
				.Select(m => new MediatorDetails
				{
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					NationalId = m.NationalId,
					Balance = m.Balance,
					Job = m.Job,
					Address = m.Address,
					BirthDate = m.BirthDate,
					Bio = m.Bio,
					Completed = m.Completed,
					JwtToken = tokenGenerator.Generate(m.Id.ToString(), Roles.Mediator),
					FirebaseToken = dto.FirebaseToken,
					Gender = m.GenderId.ToString(),
					Region = m.Region.Name,
					SocialStatus = m.SocialStatusId.ToString(),
					Status = m.StatusId.ToString(),
					ProfileImageUrl = Paths.ProfilePicture(m.Id),
					NationalIdImageUrl = Paths.NationalIdImage(m.Id),
					GeoLocation = new GeoLocationDto
					{
						Longitude = m.GeoLocation.Location.Coordinate.X,
						Latitude = m.GeoLocation.Location.Coordinate.Y,
						Details = m.GeoLocation.Details
					}
				})
				.FirstAsync(m => m.PhoneNumber == dto.PhoneNumber);

			_ = UpdateFirebaseTokenAsync(dto.PhoneNumber, dto.FirebaseToken);
			return new Success(responseDto);
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Profile()
		{
			var profile = await _context.Mediators
				.Where(m => m.Id == UserHandler.GetId(User))
				.Select(m => new ProfileDto
				{
					Name = m.Name,
					Balance = m.Balance,
					Bio = m.Bio,
					SocialStatusId = m.SocialStatusId,
					ImageUrl = Paths.ProfilePicture(m.Id)
				})
				.FirstAsync();

			return new Success(profile);
		}

		[HttpPatch("[action]")]
		public async Task<IActionResult> Profile([FromForm] CompleteProfileDto dto)
		{
			var mediator = new Mediator(UserHandler.GetId(User));
			_context.Mediators.Attach(mediator);
			dto.UpdateMediator(mediator);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpGet("profile-image/{id:min(1)}")]
		public async Task<IActionResult> ProfileImage(int id)
		{
			var image = await _context.Mediators
				.Where(m => m.Id == id)
				.Select(m => m.ProfileImage)
				.FirstOrDefaultAsync();

			return image == null ? NotFound(null) : File(image, "image/jpeg");
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

			var notificationsCount = await _context.Notifications.CountAsync(n => n.MediatorId == UserHandler.GetId(User));
			if (notificationsCount <= 0)
				return new SuccessWithPagination(Array.Empty<object>(), new Pagination(page));

			var notifications = await _context.Notifications
				.Where(n => n.MediatorId == UserHandler.GetId(User))
				.OrderBy(n => n.DateTime)
				.Select(n => new NotificationDto
				{
					Id = n.Id,
					Title = n.Title,
					Body = n.Body,
					IsRead = n.IsRead,
					TaskId = n.TaskId,
					DateTime = n.DateTime,
					ImageUrl = n.ImageUrl,
					Type = n.TypeId.ToString()
				})
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.ToArrayAsync();

			var pagination = new Pagination(page, notificationsCount, notifications.Length);
			if (notifications.Any(n => !n.IsRead))
				_ = SetNotificationsAsReadAsync(UserHandler.GetId(User));

			return new SuccessWithPagination(notifications, pagination);
		}

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> ValidateNumber([FromForm] PhoneNumberDto numberDto)
		{
			var isNumberExists = await _context.Mediators.AnyAsync(m => m.PhoneNumber == numberDto.PhoneNumber);
			return isNumberExists ? new BadRequest("Number is registered already") : new Success();
		}

		// ************************ Private methods ************************

		private async Task NewMediatorNotificationAsync(Mediator newMediator)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var mediatorsToBeNotified = await context.Mediators.Take(5)
						.Where(m => m.StatusId == StatusType.Accepted)
						.OrderBy(m => m.GeoLocation.Location.Distance(newMediator.GeoLocation.Location))
						.Select(m => new { m.Id, m.FirebaseToken })
						.ToArrayAsync();

			if (!mediatorsToBeNotified.Any())
				return;

			var notification = new Notification
			{
				Title = "New Mediator",
				Body = $"<bold>{newMediator.Name}</bold> has registered, Check him out!!!",
				TaskId = newMediator.Id,
				TypeId = Enums.NotificationType.Mediator,
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

		private async Task UpdateFirebaseTokenAsync(string phoneNumber, string token)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var mediator = await context.Mediators
				.Where(m => m.PhoneNumber == phoneNumber && m.FirebaseToken != token)
				.Select(m => new Mediator(m.Id))
				.FirstOrDefaultAsync();

			if (mediator != null)
			{
				context.Mediators.Attach(mediator);
				mediator.FirebaseToken = token;
				await context.SaveChangesAsync();
			}
		}
	}
}
