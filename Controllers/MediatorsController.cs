using System.Device.Location;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.DTOs.Mediator;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Models.Location;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.NotificationsManagement;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MediatorsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public MediatorsController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Register([FromForm] RegisterDto dto)
		{
			var errors = await ValidateMediatorAsync(dto);
			if (errors != null)
				return errors;

			var mediator = _mapper.Map<Mediator>(dto);
			var imageTask = MediatorImagesHandler.SetNationalIdImageAsync(mediator, dto.NatoinalIdImage);
			mediator.StatusId = await GetStatusIdAsync(Status.Pending);
			mediator.LocaleId = await GetLocaleIdAsync("en");
			await imageTask;
			await _context.Mediators.AddAsync(mediator);
			await _context.SaveChangesAsync();
			await SendNotificationForNewMediatorAsync(mediator);
			return new Success();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> SignIn([FromForm] SignInDto dto, [FromServices] IAuthenticationTokenGenerator tokenGenerator)
		{
			var mediator = await _context.Mediators
				.Where(m => m.PhoneNumber == dto.PhoneNumber)
				.Select(m => new Mediator
				{
					Id = m.Id,
					FirebaseToken = m.FirebaseToken,
					Status = new Models.Shared.Status { Name = m.Status.Name }
				}).FirstOrDefaultAsync();

			var errors = ValidateStatus(mediator);
			if (errors != null)
				return errors;

			if (mediator.FirebaseToken != dto.FirebaseToken)
			{
				_context.Attach(mediator);
				mediator.FirebaseToken = dto.FirebaseToken;
				await _context.SaveChangesAsync();
			}

			string fullDomain = string.Concat(Request.Scheme, "://", Request.Host, Request.PathBase.ToString().ToLower());
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
					ProfileImageUrl = string.Concat(fullDomain, "/api/mediators/profile/image"),
					NationalIdImageUrl = string.Concat(fullDomain, "/api/mediators/NationalId/image"),
					GeoLocation = new GeoLocationDto
					{
						Latitude = m.GeoLocation.Latitude,
						Longitude = m.GeoLocation.Longitude,
						Details = m.GeoLocation.Details
					}
				}).FirstAsync();

			return new Success(mediatorDto);
		}

		[Authorize]
		[HttpPatch("[action]")]
		public async Task<IActionResult> Profile([FromForm] ProfileCompletionDto dto)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var mediator = await _context.Mediators.FirstAsync(m => m.Id == userId);
			await dto.UpdateMediatorAsync(mediator);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[Authorize]
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
					ImageUrl = string.Concat(Request.Scheme, "://", Request.Host, Request.PathBase.ToString().ToLower(), "/api/mediators/profile/image")
				}).FirstAsync();

			return new Success(mediatorDto);
		}

		[Authorize]
		[HttpGet("profile/image")]
		public async Task<IActionResult> ProfileImage()
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var image = await _context.Mediators
				.Where(m => m.Id == userId)
				.Select(m => m.ProfileImage)
				.FirstOrDefaultAsync();

			return image == null ? NotFound(null) : File(image, "image/jpeg");
		}

		[Authorize]
		[HttpGet("nationalid/image")]
		public async Task<IActionResult> NationalIdImage()
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var image = await _context.Mediators
				.Where(m => m.Id == userId)
				.Select(m => m.NationalIdImage)
				.FirstOrDefaultAsync();

			return image == null ? NotFound(null) : File(image, "image/jpeg");
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> ValidateNumber([FromForm] PhoneNumberDto numberDTO)
		{
			var isNumberExists = await _context.Mediators.AnyAsync(m => m.PhoneNumber == numberDTO.PhoneNumber);
			return isNumberExists ? new BadRequest("Number is registered already") : new Success();
		}

		// ********************** Private methods **********************************

		private async Task<BadRequest> ValidateMediatorAsync(RegisterDto dto)
		{
			var mediator = await _context.Mediators
				.Select(m => new Mediator
				{
					NationalId = m.NationalId,
					PhoneNumber = m.PhoneNumber
				}).FirstOrDefaultAsync(m => m.PhoneNumber == dto.PhoneNumber || m.NationalId == dto.NationalId);

			if (mediator == null)
				return null;

			if (mediator.PhoneNumber == dto.PhoneNumber)
				return new BadRequest(nameof(dto.PhoneNumber), "Phone number already exists");
			else
				return new BadRequest(nameof(dto.NationalId), "National id already exists");
		}

		private async Task<byte> GetStatusIdAsync(string status)
		{
			return await _context.Status
				.Where(s => s.Name == status)
				.Select(s => s.Id)
				.FirstAsync();
		}

		private async Task<byte> GetLocaleIdAsync(string locale)
		{
			return await _context.Locales
				.Where(s => s.Name == locale)
				.Select(s => s.Id)
				.FirstAsync();
		}

		private BadRequest ValidateStatus(Mediator mediator)
		{
			if (mediator == null)
				return new BadRequest("Please register first");

			if (mediator.Status.Name == Status.Pending || mediator.Status.Name == Status.Submitted)
				return new BadRequest(nameof(mediator.Status.Name), "Your registeration request is pending...");

			if (mediator.Status.Name == Status.Rejected)
				return new BadRequest(nameof(mediator.Status.Name), "Your registeration request has been rejected");

			return null;
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Notify([FromForm] string token, [FromForm] string title, [FromForm] string body)
		{
			var manager = new NotificationHandler(title, body);
			await manager.SendAsync(token);
			return new Success();
		}

		private async Task SendNotificationForNewMediatorAsync(Mediator mediator)
		{
			var geoLocations = await _context.Mediators
							.Where(m => m.Status.Name == Status.Accepted)
							.Select(m => new GeoLocation
							{
								Id = m.GeoLocation.Id,
								Latitude = m.GeoLocation.Latitude,
								Longitude = m.GeoLocation.Longitude
							}).ToArrayAsync();

			var mediatorCoordinate = new GeoCoordinate(mediator.GeoLocation.Latitude, mediator.GeoLocation.Longitude);
			var closestLocationsId = geoLocations
				.OrderBy(l => mediatorCoordinate.GetDistanceTo(new GeoCoordinate(l.Latitude, l.Longitude)))
				.Select(l => l.Id)
				.Take(5);

			var tokens = await _context.Mediators
				.Where(m => closestLocationsId.Contains(m.GeoLocationId))
				.Select(m => m.FirebaseToken)
				.ToArrayAsync();

			var handler = new NotificationHandler("New Mediator", "Please check the new mediator");
			await handler.SendAsync(tokens);
		}
	}
}
