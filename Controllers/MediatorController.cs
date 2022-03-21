using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Mediator;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MediatorController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public MediatorController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpPost("[action]")]
		[RequestFormLimits(MultipartBodyLengthLimit = 1024 * 1024)]
		public async Task<IActionResult> Register([FromForm] MediatorRegister model)
		{
			var result = await IsMediatorRegisteredAsync(model);
			if (result != null)
				return result;

			var pendingStatusId = _context.Status
				.Where(s => s.Name == nameof(Utilities.StaticStrings.Status.Pending))
				.Select(s => s.Id)
				.FirstAsync();

			var mediator = _mapper.Map<Mediator>(model);
			await mediator.SetNationalIdImageAsync(model.NatoinalIdImage);
			mediator.StatusId = await pendingStatusId;
			await _context.Mediators.AddAsync(mediator);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[Authorize]
		[HttpPatch("[action]")]
		[RequestFormLimits(MultipartBodyLengthLimit = 1024 * 1024)]
		public async Task<IActionResult> CompleteProfile([FromForm] MediatorRegisterCompletion model)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var mediator = await _context.Mediators.FirstAsync(m => m.Id == userId);
			await model.UpdateMediatorAsync(mediator);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> SignIn([FromForm] MediatorSignIn model, [FromServices] IAuthenticationTokenGenerator tokenGenerator)
		{
			var mediator = await _context.Mediators
				.Where(m => m.PhoneNumber == model.PhoneNumber)
				.Select(m => new Mediator
				{
					Id = m.Id,
					Name = m.Name,
					NotificationToken = m.NotificationToken,
					Status = new Status { Name = m.Status.Name }
				}).FirstOrDefaultAsync();

			var errors = ValidateStatus(mediator);
			if (errors != null)
				return errors;

			if (mediator.NotificationToken != model.FirebaseToken)
			{
				_context.Attach(mediator);
				mediator.NotificationToken = model.FirebaseToken;
				await _context.SaveChangesAsync();
			}

			var token = tokenGenerator.Generate(mediator.Id.ToString(), new[]
				{
					new KeyValuePair<string, string>("PhoneNumber", model.PhoneNumber),
					new KeyValuePair<string, string>("IMEI", model.IMEI)
				});
			return new Success(token);
		}

		[Authorize]
		[HttpGet("[action]")]
		public async Task<IActionResult> Profile()
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var mediator = await _context.Mediators
				.Where(m => m.Id == userId)
				.Select(m => new Mediator
				{
					Name = m.Name,
					Bio = m.Bio,
					SocialStatusId = m.SocialStatusId
				}).FirstAsync();

			var mediatorProfile = _mapper.Map<MediatorProfile>(mediator);
			mediatorProfile.ImageUrl = string.Concat(Request.Scheme, "://", Request.Host, Request.PathBase, "/api/Mediator/ProfileImage");
			return new Success(mediatorProfile);
		}

		[Authorize]
		[HttpGet("[action]")]
		public async Task<IActionResult> ProfileImage()
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var image = await _context.Mediators
				.Where(m => m.Id == userId)
				.Select(m => m.ProfileImage)
				.FirstOrDefaultAsync();

			if (image == null)
				return NotFound(null);

			return File(image, "image/jpeg");
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> ValidateNumber([FromForm] MediatorPhoneNumber numberDTO)
		{
			var isNumberRegistered = await _context.Mediators.AsNoTracking().AnyAsync(m => m.PhoneNumber == numberDTO.PhoneNumber);
			return isNumberRegistered ? new BadRequest("Number is registered already") : new Success();
		}

		private async Task<BadRequest> IsMediatorRegisteredAsync(MediatorRegister model)
		{
			var mediator = await _context.Mediators
				.Select(m => new Mediator
				{
					NationalId = m.NationalId,
					PhoneNumber = m.PhoneNumber
				}).FirstOrDefaultAsync(m => m.PhoneNumber == model.PhoneNumber || m.NationalId == model.NationalId);

			if (mediator == null)
				return null;

			if (mediator.PhoneNumber == model.PhoneNumber)
				return new BadRequest(nameof(model.PhoneNumber), "Phone number already exists");
			else
				return new BadRequest(nameof(model.NationalId), "National id already exists");
		}

		private BadRequest ValidateStatus(Mediator mediator)
		{
			if (mediator == null)
				return new BadRequest("Please register first");

			if (mediator.Status.Name == Utilities.StaticStrings.Status.Pending || mediator.Status.Name == Utilities.StaticStrings.Status.Submitted)
				return new BadRequest(nameof(mediator.Status.Name), "Your registeration request is pending...");

			if (mediator.Status.Name == Utilities.StaticStrings.Status.Rejected)
				return new BadRequest(nameof(mediator.Status.Name), "Your registeration request has been rejected");

			return null;
		}
	}
}
