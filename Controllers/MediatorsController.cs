using System;
using System.Device.Location;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.DTOs.Case;
using GraduationProjectAPI.DTOs.Mediator;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Models.Location;
using GraduationProjectAPI.Models.Reviews;
using GraduationProjectAPI.Models.Shared;
using GraduationProjectAPI.Utilities;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
using GraduationProjectAPI.Utilities.CustomApiResponses;
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
		private readonly IMapper _mapper;
		private readonly IMemoryCache _memoryCache;
		private const string _profilePictureUrl = "/api/mediators/profile-image/";
		private const string _nationalIdImageUrl = "/api/mediators/nationalid-image/";

		public MediatorsController(ApplicationDbContext context, IMapper mapper, IMemoryCache memoryCache)
		{
			_context = context;
			_mapper = mapper;
			_memoryCache = memoryCache;
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

			var imageUrl = RequestPath().Append("/api/cases/image/");
			foreach (var @case in cases)
				@case.ImagesUrl = images.Where(i => i.CaseId == @case.Id).Select(i => imageUrl.Append(i.Id).ToString()).ToArray();

			return new Success(cases);
		}

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> Register([FromForm] RegisterDto dto, [FromServices] IServiceScopeFactory scopeFactory)
		{
			var result = await ValidateMediatorAsync(dto.PhoneNumber, dto.NationalId);
			if (result != null)
				return result;

			var mediator = _mapper.Map<Mediator>(dto);
			mediator.StatusId = (byte)StatusType.Pending;
			mediator.LocaleId = (byte)LocaleType.EN;
			await _context.Mediators.AddAsync(mediator);
			await _context.SaveChangesAsync();
			_ = SendNotificationForNewMediatorAsync(mediator, scopeFactory);
			return new Success();
		}

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> SignIn([FromForm] SignInDto dto, [FromServices] IAuthenticationTokenGenerator tokenGenerator)
		{
			var mediator = await _context.Mediators
				.Where(m => m.PhoneNumber == dto.PhoneNumber)
				.Select(m => new Mediator
				{
					Id = m.Id,
					FirebaseToken = m.FirebaseToken,
					Status = new Status { Name = m.Status.Name }
				}).FirstOrDefaultAsync();

			var error = ValidateStatus(mediator);
			if (error != null)
				return error;

			if (mediator.FirebaseToken != dto.FirebaseToken)
			{
				_context.Attach(mediator);
				mediator.FirebaseToken = dto.FirebaseToken;
				await _context.SaveChangesAsync();
			}

			var mediatorDto = await _context.Mediators
				.Where(m => m.Id == mediator.Id)
				.Select(m => new MediatorDto
				{
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					NationalId = m.NationalId,
					Job = m.Job,
					Address = m.Address,
					BirthDate = m.BirthDate,
					Bio = m.Bio,
					Region = m.Region.Name,
					Gender = m.Gender.Name,
					SocialStatus = m.SocialStatus.Name,
					Locale = m.Locale.Name,
					Completed = m.Completed,
					Status = mediator.Status.Name,
					FirebaseToken = mediator.FirebaseToken,
					JwtToken = tokenGenerator.Generate(mediator.Id.ToString()),
					ProfileImageUrl = RequestPath().Append(_profilePictureUrl).ToString() + m.Id,
					NationalIdImageUrl = RequestPath().Append(_nationalIdImageUrl).ToString() + m.Id,
					GeoLocation = new GeoLocationDto
					{
						Latitude = m.GeoLocation.Latitude,
						Longitude = m.GeoLocation.Longitude,
						Details = m.GeoLocation.Details
					}
				}).FirstAsync();

			return new Success(mediatorDto);
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Profile()
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var mediatorDto = await _context.Mediators
				.Where(m => m.Id == userId)
				.Select(m => new ProfileDto
				{
					Name = m.Name,
					Bio = m.Bio,
					SocialStatusId = m.SocialStatusId,
					ImageUrl = RequestPath().Append(_profilePictureUrl).ToString() + m.Id
				}).FirstAsync();

			return new Success(mediatorDto);
		}

		[HttpPatch("[action]")]
		public async Task<IActionResult> Profile([FromForm] ProfileCompletionDto dto)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var mediator = await _context.Mediators.FirstAsync(m => m.Id == userId);
			dto.UpdateMediator(mediator);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpGet("profile-image/{id:min(1)}")]
		public async Task<IActionResult> ProfileImage(uint id)
		{
			byte[] image;
			if (_memoryCache.TryGetValue(nameof(image) + id, out image))
				return File(image, "image/jpeg");

			image = await _context.Mediators
				.Where(m => m.Id == id)
				.Select(m => m.ProfileImage)
				.FirstOrDefaultAsync();

			if (image == null)
				return NotFound(null);

			_memoryCache.Set(nameof(image) + id, image, DateTimeOffset.Now.AddMinutes(1));
			return File(image, "image/jpeg");
		}

		[HttpGet("nationalid-image/{id:min(1)}")]
		public async Task<IActionResult> NationalIdImage(uint id)
		{
			var image = await _context.Mediators
				.Where(m => m.Id == id)
				.Select(m => m.NationalIdImage)
				.FirstOrDefaultAsync();

			return image == null ? NotFound(null) : File(image, "image/jpeg");
		}

		[HttpGet("pending-mediators")]
		public async Task<IActionResult> PendingMediators()
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var userGeoCoordinate = await _context.Mediators
				.Where(m => m.Id == userId)
				.Join(_context.GeoLocations,
					m => m.GeoLocationId,
					g => g.Id,
					(m, g) => new GeoCoordinate(g.Latitude, g.Longitude))
				.FirstAsync();

			var geoLocations = await _context.Mediators
				.Where(m => m.StatusId == (byte)StatusType.Pending && !m.ReviewsAboutMe.Any(r => r.ReviewerId == userId))
				.Select(m => new GeoLocation
				{
					Id = m.GeoLocationId,
					Latitude = m.GeoLocation.Latitude,
					Longitude = m.GeoLocation.Longitude
				})
				.ToArrayAsync();

			var closestLocationsId = geoLocations
				.OrderBy(l => userGeoCoordinate.GetDistanceTo(new GeoCoordinate(l.Latitude, l.Longitude)))
				.Select(l => l.Id)
				.Take(5);

			var pendingMediators = await _context.Mediators
				.Where(m => closestLocationsId.Contains(m.GeoLocationId))
				.Select(m => new
				{
					Id = m.Id,
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					Address = m.Address,
					ImageUrl = RequestPath().Append(_profilePictureUrl).ToString() + m.Id
				})
				.ToArrayAsync();

			return new Success(pendingMediators);
		}

		[HttpGet("pending-mediators/{id:min(1)}")]
		public async Task<IActionResult> PendingMediators(int id)
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
					ImageUrl = RequestPath().Append(_profilePictureUrl).ToString() + m.Id
				})
				.FirstOrDefaultAsync();

			return new Success();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Reviews([FromForm] ReviewDto dto)
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

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> ValidateNumber([FromForm] PhoneNumberDto numberDTO)
		{
			var isNumberExists = await _context.Mediators.AnyAsync(m => m.PhoneNumber == numberDTO.PhoneNumber);
			return isNumberExists ? new BadRequest("Number is registered already") : new Success();
		}

		// ********************** Private methods **********************************

		private async Task<BadRequest> ValidateMediatorAsync(string phoneNumber, string nationalId)
		{
			var mediator = await _context.Mediators
				.Select(m => new Mediator
				{
					NationalId = m.NationalId,
					PhoneNumber = m.PhoneNumber
				}).FirstOrDefaultAsync(m => m.PhoneNumber == phoneNumber || m.NationalId == nationalId);

			if (mediator == null)
				return null;

			if (mediator.PhoneNumber == phoneNumber)
				return new BadRequest(nameof(phoneNumber), "Phone number already exists");
			else
				return new BadRequest(nameof(nationalId), "National id already exists");
		}

		private BadRequest ValidateStatus(Mediator mediator)
		{
			if (mediator == null)
				return new BadRequest("Please register first");

			if (mediator.StatusId == (byte)StatusType.Pending || mediator.StatusId == (byte)StatusType.Submitted)
				return new BadRequest(nameof(mediator.Status.Name), "Your registeration request is pending...");

			if (mediator.StatusId == (byte)StatusType.Rejected)
				return new BadRequest(nameof(mediator.Status.Name), "Your registeration request has been rejected");

			return null;
		}

		private StringBuilder RequestPath()
		{
			return new StringBuilder(Request.Scheme).Append("://").Append(Request.Host).Append(Request.PathBase.ToString());
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Notify([FromForm] string token, [FromForm] string title, [FromForm] string body)
		{
			var manager = new NotificationHandler(title, body);
			await manager.SendAsync(token);
			return new Success();
		}

		private async Task SendNotificationForNewMediatorAsync(Mediator mediator, IServiceScopeFactory scopeFactory)
		{
			using var scope = scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var geoLocations = await context.Mediators
						.Where(m => m.Status.Name == StatusType.Accepted.ToString())
						.Select(m => new GeoLocation
						{
							Id = m.GeoLocation.Id,
							Latitude = m.GeoLocation.Latitude,
							Longitude = m.GeoLocation.Longitude
						}).ToArrayAsync();

			var mediatorCoordinate = new GeoCoordinate(mediator.GeoLocation.Latitude,
				mediator.GeoLocation.Longitude);

			var closestLocationsId = geoLocations
				.OrderBy(l => mediatorCoordinate.GetDistanceTo(
					new GeoCoordinate(l.Latitude, l.Longitude)))
				.Select(l => l.Id)
				.Take(5);

			var mediatorsToBeNotified = await context.Mediators
				.Where(m => closestLocationsId.Contains(m.GeoLocationId))
				.Select(m => new Mediator
				{
					Id = m.Id,
					FirebaseToken = m.FirebaseToken
				})
				.ToArrayAsync();

			var handler = new NotificationHandler("New Mediator", "Please check the new mediator");
			await handler.SendAsync(mediatorsToBeNotified.Select(m => m.FirebaseToken));

			foreach (var med in mediatorsToBeNotified)
			{
				await context.Notifications.AddAsync(new Notification()
				{
					Title = "New Mediator",
					Body = "Please check the new mediator",
					MediatorId = med.Id
				});
			}

			await context.SaveChangesAsync();
		}
	}
}
