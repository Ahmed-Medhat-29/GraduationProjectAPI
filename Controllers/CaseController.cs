using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Case;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CaseController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public CaseController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[Authorize]
		[HttpPost("[action]")]
		public async Task<IActionResult> Add([FromForm] CaseAddingDto caseDto)
		{
			var result = await IsCasesAddedAsync(caseDto);
			if (result != null)
				return result;

			var pendingStatusId = _context.Status
				.Where(s => s.Name == nameof(Utilities.StaticStrings.Status.Pending))
				.Select(s => s.Id)
				.FirstAsync();

			var newCase = _mapper.Map<Case>(caseDto);
			var settingImageTask = newCase.SetNationalIdImageAsync(caseDto.NationalIdImage);

			if (caseDto.OptionalImages != null)
				newCase.AddOptionalImages(caseDto.OptionalImages);

			newCase.MediatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			newCase.StatusId = await pendingStatusId;
			await settingImageTask;
			await _context.AddAsync(newCase);
			await _context.SaveChangesAsync();
			return new Success();
		}

		private async Task<BadRequest> IsCasesAddedAsync(CaseAddingDto model)
		{
			var caseDb = await _context.Cases
				.Select(m => new Case
				{
					NationalId = m.NationalId,
					PhoneNumber = m.PhoneNumber
				}).FirstOrDefaultAsync(m => m.PhoneNumber == model.PhoneNumber || m.NationalId == model.NationalId);

			if (caseDb == null)
				return null;

			if (caseDb.PhoneNumber == model.PhoneNumber)
				return new BadRequest(nameof(model.PhoneNumber), "Phone number already exists");
			else
				return new BadRequest(nameof(model.NationalId), "National id already exists");
		}
	}
}
