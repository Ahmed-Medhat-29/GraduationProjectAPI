using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request.Donators;
using GraduationProjectAPI.DTOs.Response.Donators;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraduationProjectAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = Roles.Donator)]
	public class DonatorsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceScopeFactory _scopeFactory;

		public DonatorsController(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
		{
			_context = context;
			_scopeFactory = scopeFactory;
		}

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> Register([FromForm] RegisterDto dto)
		{
			var donator = dto.ToDonator();
			await _context.Donators.AddAsync(donator);
			await _context.SaveChangesAsync();
			return new Success();
		}

		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> SignIn([FromForm] SignInRequestDto dto, [FromServices] IAuthenticationTokenGenerator tokenGenerator)
		{
			var responseDto = await _context.Donators
				.Select(d => new DonatorDetails
				{
					Name = d.Name,
					PhoneNumber = d.PhoneNumber,
					JwtToken = tokenGenerator.Generate(d.Id.ToString(), Roles.Donator),
					FirebaseToken = dto.FirebaseToken
				})
				.FirstAsync(d => d.PhoneNumber == dto.PhoneNumber);

			_ = UpdateFirebaseTokenAsync(dto.PhoneNumber, dto.FirebaseToken);
			return new Success(responseDto);
		}

		// ************************ Private methods ************************

		private async Task UpdateFirebaseTokenAsync(string phoneNumber, string token)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var donator = await context.Donators
				.Where(d => d.PhoneNumber == phoneNumber && d.FirebaseToken != token)
				.Select(d => new Donator(d.Id))
				.FirstOrDefaultAsync();

			if (donator != null)
			{
				context.Donators.Attach(donator);
				donator.FirebaseToken = token;
				await context.SaveChangesAsync();
			}
		}
	}
}
