using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Common;
using GraduationProjectAPI.DTOs.Response.Cases;
using GraduationProjectAPI.DTOs.Response.Mediators;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.General;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[Route("api")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}

		[Authorize(Roles = Roles.Mediator)]
		[HttpGet("[action]/[controller]")]
		public async Task<IActionResult> Mediators()
		{
			var user = await _context.Mediators
				.Where(m => m.Id == UserHandler.GetId(User))
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
					FirebaseToken = m.FirebaseToken,
					Gender = m.GenderId.ToString(),
					Region = m.Region.Name,
					RegionId = m.RegionId,
					SocialStatus = m.SocialStatusId.ToString(),
					Locale = m.LocaleId.ToString(),
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
				.FirstAsync();

			if (user.RegionId == null)
				return new BadRequest("Please complete your profile and choose your region");

			user.JwtToken = await HttpContext.GetTokenAsync("Bearer", "access_token");

			var query = _context.Cases.Take(5)
				.Where(c => c.StatusId == StatusType.Accepted)
				.OrderByDescending(c => c.DateRequested);

			Expression<Func<Case, CaseElementDto>> caseElementDto = c =>
				new CaseElementDto
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
				};

			var urgentCases = await query
				.Where(c => c.PriorityId == PriorityType.Urgent)
				.Select(caseElementDto)
				.ToArrayAsync();

			var myCases = await query
				.Where(c => c.MediatorId == UserHandler.GetId(User))
				.Select(caseElementDto)
				.ToArrayAsync();

			var areaCases = await query
				.Where(c => c.RegionId == user.RegionId)
				.Select(caseElementDto)
				.ToArrayAsync();

			return new Success(new { urgentCases, myCases, areaCases, user });
		}

		[Authorize(Roles = Roles.Donator)]
		[HttpGet("[action]/[controller]")]
		public IActionResult Donators()
		{
			return new Success("Hi, I am donator");
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> FAQ()
		{
			return new Success(await _context.FAQs
				.Select(f => new { f.Title, f.Description })
				.ToArrayAsync());
		}
	}
}
