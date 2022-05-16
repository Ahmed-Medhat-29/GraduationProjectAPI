using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Response.Payments;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.General;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = Roles.Mediator)]
	public class PaymentsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public PaymentsController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Wallet()
		{
			var receive = await _context.CasePayments
				.Where(cp => cp.MediatorId == GetUserId())
				.Select(cp => new TransactionElement
				{
					CaseId = cp.CaseId,
					Name = cp.Case.Name,
					Amount = cp.Amount,
					DateTime = cp.DateSubmitted,
					ImageUrl = cp.Case.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				}).ToArrayAsync();

			var paid = await _context.CasePayments
				.Where(cp => cp.MediatorId == GetUserId() && cp.DateDelivered != null)
				.Select(cp => new TransactionElement
				{
					CaseId = cp.CaseId,
					Name = cp.Case.Name,
					Amount = cp.Amount * -1,
					DateTime = (DateTime)cp.DateDelivered,
					ImageUrl = cp.Case.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				}).ToArrayAsync();

			var transactions = receive.Union(paid).OrderByDescending(t => t.DateTime);
			var wallet = await _context.Mediators
				.Where(m => m.Id == GetUserId())
				.Select(m => new WalletDto
				{
					Name = m.Name,
					Balance = m.Balance,
					ImageUrl = Paths.ProfilePicture(m.Id),
					Transactions = new List<TransactionElement>(transactions)
				})
				.FirstAsync();

			return new Success(wallet);
		}

		[HttpGet("case-history/{id:min(1)}")]
		public async Task<IActionResult> CaseHistory(int id)
		{
			var total = await _context.Cases.Where(c => c.Id == id)
				.Select(c => c.NeededMoneyAmount)
				.FirstOrDefaultAsync();

			if (total == 0)
				return new BadRequest("Case not found");

			var history = await _context.CasePayments
				.Where(cp => cp.CaseId == id && cp.DateDelivered != null)
				.Select(cp => new PaymentElementDto
				{
					Name = cp.Mediator.Name,
					Amount = cp.Amount,
					Datetime = (DateTime)cp.DateDelivered,
					ImageUrl = cp.Case.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()

				}).ToArrayAsync();

			var paid = 0;
			foreach (var item in history)
				paid += item.Amount;

			var paymentHistory = new CasePaymentHistoryDto
			{
				Total = total,
				Paid = paid,
				Remaining = total - paid,
				History = history
			};

			return new Success(paymentHistory);
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Cases(int page)
		{
			if (page <= 0) return NotFound(null);

			var casesCount = await _context.CasePayments
				.CountAsync(cp => cp.DateDelivered == null && cp.MediatorId == GetUserId());

			if (casesCount <= 0)
				return new SuccessWithPagination(Array.Empty<object>(), new Pagination(page));

			var cases = await _context.CasePayments
				.Where(cp => cp.DateDelivered == null && cp.MediatorId == GetUserId())
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.Select(cp => new CasePaymentElementDto
				{
					Id = cp.Id,
					Title = cp.Case.Name,
					Amount = cp.Amount,
					Total = cp.Case.NeededMoneyAmount,
					PhoneNumber = cp.Case.PhoneNumber,
					MediatorName = cp.Case.Mediator.Name,
					CaseName = cp.Case.Name,
					Age = (short)(cp.Case.PaymentDate - DateTime.Now).TotalDays,
					ImageUrl = cp.Case.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				}).ToArrayAsync();

			foreach (var @case in cases)
			{
				var caseId = await _context.CasePayments
					.Where(cp => cp.Id == @case.Id)
					.Select(cp => cp.CaseId)
					.FirstAsync();

				var amounts = await _context.CasePayments
					.Where(cp => cp.CaseId == caseId && cp.DateDelivered != null)
					.Select(cp => cp.Amount)
					.ToArrayAsync();

				@case.Contributers = amounts.Count();

				foreach (var amount in amounts)
					@case.Paid += amount;
			}


			return new SuccessWithPagination(cases, new Pagination(page, casesCount, cases.Length));
		}

		[HttpPost("[action]/{id:min(1)}")]
		public async Task<IActionResult> Confirm(int id)
		{
			var casePayment = await _context.CasePayments
				.Select(cp => new CasePayment
				{
					Id = cp.Id,
					Amount = cp.Amount,
					DateDelivered = cp.DateDelivered
				})
				.FirstOrDefaultAsync(cp => cp.Id == id);

			if (casePayment == null)
				return new BadRequest("Transaction was not found");

			if (casePayment.DateDelivered != null)
				return new BadRequest("You have paid this case already");

			var mediator = await _context.Mediators
				.Select(m => new Mediator
				{
					Id = m.Id,
					Balance = m.Balance
				}).FirstAsync(m => m.Id == GetUserId());

			if (mediator.Balance < casePayment.Amount)
				return new BadRequest("Insufficient balance");

			_context.CasePayments.Attach(casePayment);
			casePayment.DateDelivered = DateTime.Now;

			_context.Mediators.Attach(mediator);
			mediator.Balance -= casePayment.Amount;

			await _context.SaveChangesAsync();
			return new Success();
		}

		private int GetUserId()
		{
			return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
		}
	}
}
