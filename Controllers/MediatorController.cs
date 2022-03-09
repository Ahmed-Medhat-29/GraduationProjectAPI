using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Mediator;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
using GraduationProjectAPI.Utilities.Customs.ApiResponses;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Register([FromForm] MediatorRegister model)
		{
			var nationalIdImage = model.NatoinalIdImage;
			if (nationalIdImage == null || nationalIdImage.Length <= 0)
				return new BadRequest(nameof(model.NatoinalIdImage), "Please provide an image of your national ID");

			var badRequest = await IsMediatorRegisteredAsync(model);
			if (badRequest != null)
				return badRequest;

			var pendingStatusId = _context.Status
				.Where(s => s.Name == nameof(Utilities.StaticStrings.Status.Pending))
				.Select(s => s.Id)
				.FirstAsync();

			var mediator = _mapper.Map<Mediator>(model);
			mediator.StatusId = await pendingStatusId;
			await _context.Mediators.AddAsync(mediator);
			await _context.SaveChangesAsync();

			await mediator.SetNationalIdImageNameAsync(nationalIdImage);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[Authorize]
		[HttpPatch("[action]")]
		[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
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
					Status = new Models.Status { Name = m.Status.Name }
				}).FirstOrDefaultAsync();

			var error = CheckStatusValidation(mediator);
			if (error != null)
				return error;

			var token = tokenGenerator.Generate(mediator.Id.ToString(), model.IMEI, model.FirebaseToken);
			return new Success(token);
		}

		[Authorize]
		[HttpGet("[action]")]
		[ProducesResponseType(typeof(MediatorProfile), StatusCodes.Status200OK)]
		public async Task<IActionResult> Profile()
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var mediator = await _context.Mediators
				.Where(m => m.Id == userId)
				.Select(m => new Mediator
				{
					Name = m.Name,
					Bio = m.Bio,
					ProfileImageName = m.ProfileImageName,
					SocialStatusId = m.SocialStatusId
				}).FirstAsync();

			var mediatorProfile = _mapper.Map<MediatorProfile>(mediator);
			if (!string.IsNullOrWhiteSpace(mediator.ProfileImageName))
				mediatorProfile.ImageUrl = string.Concat(Request.Scheme, "://", Request.Host, Request.PathBase, "/", ImagePath.Profile.Replace('\\', '/'), mediator.ProfileImageName);

			return new Success(mediatorProfile);
		}

		private async Task<BadRequest> IsMediatorRegisteredAsync(MediatorRegister model)
		{
			var mediator = await _context.Mediators.AsNoTracking()
				.Where(m => m.NationalId == model.NationalId || m.PhoneNumber == model.PhoneNumber)
				.Select(m => new Mediator
				{
					NationalId = m.NationalId,
					PhoneNumber = m.PhoneNumber
				}).FirstOrDefaultAsync();

			if (mediator == null)
				return null;

			if (mediator.NationalId == model.NationalId)
				return new BadRequest(nameof(model.NationalId), "National id already exists");
			else
				return new BadRequest(nameof(model.PhoneNumber), "Phone number already exists");
		}

		private BadRequest CheckStatusValidation(Mediator mediator)
		{
			if (mediator == null)
				return new BadRequest(nameof(mediator.PhoneNumber), "Please register first");

			if (mediator.Status.Name == Utilities.StaticStrings.Status.Pending || mediator.Status.Name == Utilities.StaticStrings.Status.Submitted)
				return new BadRequest(nameof(mediator.Status.Name), "Your registeration request is pending...");

			if (mediator.Status.Name == Utilities.StaticStrings.Status.Rejected)
				return new BadRequest(nameof(mediator.Status.Name), "Your registeration request has been rejected");

			return null;
		}
	}
}
