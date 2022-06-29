using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request.Payments;
using GraduationProjectAPI.DTOs.Response.Payments;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Models.CaseProperties;
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
			var payments = await _context.CasePayments
				.Where(cp => cp.MediatorId == UserHandler.GetId(User))
				.Select(cp => new
				{
					CaseId = cp.CaseId,
					Name = cp.Case.Name,
					Amount = cp.Amount,
					DateSubmitted = cp.DateSubmitted,
					DateDelivered = cp.DateDelivered,
					ImageUrl = cp.Case.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				}).ToArrayAsync();

			var walletTask = _context.Mediators
				.Where(m => m.Id == UserHandler.GetId(User))
				.Select(m => new WalletDto
				{
					Name = m.Name,
					Balance = m.Balance,
					ImageUrl = Paths.ProfilePicture(m.Id)
				})
				.FirstAsync();

			var transactions = payments
				.Select(p => new TransactionElement
				{
					CaseId = p.CaseId,
					Name = p.Name,
					Amount = p.Amount,
					DateTime = p.DateSubmitted,
					ImageUrl = p.ImageUrl
				})
				.Union(payments.Where(p => p.DateDelivered != null)
					.Select(p => new TransactionElement
					{
						CaseId = p.CaseId,
						Name = p.Name,
						Amount = p.Amount * -1,
						DateTime = (DateTime)p.DateDelivered,
						ImageUrl = p.ImageUrl
					})
				)
				.OrderByDescending(t => t.DateTime).ToArray();

			var wallet = await walletTask;
			wallet.Transactions = transactions;
			return new Success(wallet);
		}

		[HttpGet("case-history/{id:min(1)}")]
		public async Task<IActionResult> CaseHistory(int id)
		{
			var @case = await _context.Cases
				.Where(c => c.Id == id)
				.Select(c => new
				{
					c.NeededMoneyAmount,
					c.PaymentDate,
					c.CurrentRound,
					c.PeriodId
				})
				.FirstOrDefaultAsync();

			if (@case == null)
				return new BadRequest("Case not found");

			IEnumerable<PaymentElementDto> history = await _context.CasePayments
				.Where(cp => cp.CaseId == id && cp.DateDelivered != null)
				.Select(cp => new PaymentElementDto
				{
					Name = cp.Mediator.Name,
					Amount = cp.Amount,
					RoundNumber = cp.RoundNnumber,
					Datetime = (DateTime)cp.DateDelivered,
					ImageUrl = cp.Case.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()

				}).ToArrayAsync();

			var paymentHistorys = new List<CasePaymentHistoryDto>();
			if (@case.PeriodId == Enums.PeriodType.OneTime)
			{
				var paymentHistory = new CasePaymentHistoryDto
				{
					Total = @case.NeededMoneyAmount,
					Paid = history.Sum(h => h.Amount),
					PaymentDate = @case.PaymentDate,
					History = history
				};

				paymentHistorys.Add(paymentHistory);
				return new Success(paymentHistorys);
			}

			for (int i = (int)@case.CurrentRound; i > 0; i--)
			{
				var paymentHistory = new CasePaymentHistoryDto
				{
					Total = @case.NeededMoneyAmount,
					Paid = history.Where(h => h.RoundNumber == i).Sum(h => h.Amount),
					PaymentDate = @case.PaymentDate.AddMonths(i - (int)@case.CurrentRound),
					History = history.Where(h => h.RoundNumber == i).ToArray()
				};

				paymentHistorys.Add(paymentHistory);
			}

			return new Success(paymentHistorys);
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> Cases(int page)
		{
			if (page <= 0) return NotFound(null);

			var casesCount = await _context.CasePayments
				.CountAsync(cp => cp.DateDelivered == null && cp.MediatorId == UserHandler.GetId(User));

			if (casesCount <= 0)
				return new SuccessWithPagination(Array.Empty<object>(), new Pagination(page));

			var cases = await _context.CasePayments
				.Where(cp => cp.DateDelivered == null && cp.MediatorId == UserHandler.GetId(User))
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

		[HttpPost("[action]")]
		public async Task<IActionResult> Confirm([FromForm] CasePaymentConfirmationDto dto)
		{
			var casePayment = await _context.CasePayments
				.Select(cp => new CasePayment
				{
					Id = cp.Id,
					Amount = cp.Amount,
					DateDelivered = cp.DateDelivered
				})
				.FirstOrDefaultAsync(cp => cp.Id == dto.Id);

			if (casePayment == null)
				return new BadRequest("Transaction was not found");

			if (casePayment.DateDelivered != null)
				return new BadRequest("You have paid this case already");

			var mediator = await _context.Mediators
				.Select(m => new Mediator
				{
					Id = m.Id,
					Balance = m.Balance
				}).FirstAsync(m => m.Id == UserHandler.GetId(User));

			if (mediator.Balance < casePayment.Amount)
				return new BadRequest("Insufficient balance");

			_context.CasePayments.Attach(casePayment);
			casePayment.DateDelivered = DateTime.Now;
			casePayment.Details = dto.Details;

			_context.Mediators.Attach(mediator);
			mediator.Balance -= casePayment.Amount;

			await _context.SaveChangesAsync();
			return new Success();
		}
	}
}
