using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.DTOs.Response.Cases;
using GraduationProjectAPI.DTOs.Response.Payments;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.General;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Utilities.ExtensionMethods
{
	public static class CasesQueryExtensions
	{
		public static async Task<CaseElementDto[]> SelectCaseElementDtoAsync(this IQueryable<Case> query)
		{
			return await query
				.OrderByDescending(c => c.DateRequested)
				.Select(c => new CaseElementDto
				{
					Id = c.Id,
					Name = c.Name,
					Title = c.Title,
					Priority = c.Priority.Name,
					Age = (short)(c.PaymentDate - DateTime.Now).TotalDays,
					FundRaised = 4000,
					TotalNeeded = c.NeededMoneyAmount,
					NumberOfContributer = 32,
					ImageUrl = c.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				}).ToArrayAsync();
		}

		public static async Task<CaseElementDto[]> SelectCaseElementDtoAsync(this IQueryable<Case> query, int page)
		{
			return await query
				.OrderByDescending(c => c.DateRequested)
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.Select(c => new CaseElementDto
				{
					Id = c.Id,
					Name = c.Name,
					Title = c.Title,
					Priority = c.Priority.Name,
					Age = (short)(c.PaymentDate - DateTime.Now).TotalDays,
					FundRaised = 4000,
					TotalNeeded = c.NeededMoneyAmount,
					NumberOfContributer = 32,
					ImageUrl = c.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				}).ToArrayAsync();
		}

		public static async Task<CaseDetailsDto> SelectCaseInfoDtoAsync(this IQueryable<Case> query, int id)
		{
			return await query.Select(c =>
				new CaseDetailsDto
				{
					Id = c.Id,
					Title = c.Title,
					Story = c.Story,
					Datetime = c.DateRequested,
					TotalNeeded = c.NeededMoneyAmount,
					Mediator = new CaseMediatorDto
					{
						Id = c.MediatorId,
						Name = c.Mediator.Name,
						ImageUrl = Paths.ProfilePicture(c.MediatorId)
					},
					History = c.CasePayments
						.Where(cp => cp.DateDelivered != null)
						.Select(cp => new PaymentElementDto
						{
							Name = cp.Mediator.Name,
							Amount = cp.Amount,
							Datetime = (DateTime)cp.DateDelivered,
							ImageUrl = cp.Case.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
						})
				})
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public static async Task<IEnumerable<string>> SelectCaseImagesUrlsAsync(this IQueryable<Case> query, int id)
		{
			return await query.Where(c => c.Id == id)
				.Select(c => c.Images.Select(i => Paths.CaseImage(i.Id)))
				.FirstOrDefaultAsync();
		}

		public static async Task<ReviewCaseTaskElementDto[]> SelectCaseTaskElementDtoAsync(this IQueryable<Case> query, int id, int page)
		{
			return await query.Where(c => c.StatusId == StatusType.Pending && c.MediatorId != id && !c.CaseReviews.Any(r => r.MediatorId == id))
				.OrderBy(c => c.DateRequested)
				.Select(c => new ReviewCaseTaskElementDto
				{
					Id = c.Id,
					Title = c.Title,
					NeededMoneyAmount = c.NeededMoneyAmount,
					Age = (short)(DateTime.Now - c.DateRequested).Days,
					Period = c.PeriodId.ToEnumString(),
					Details = c.GeoLocation.Details,
					ImageUrl = c.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				})
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.ToArrayAsync();
		}

		public static async Task<ReviewCaseTaskDetailsDto> SelectCaseTaskDetailsDtoAsync(this IQueryable<Case> query, int caseId, int userId)
		{
			return await query.Where(c => c.Id == caseId && c.StatusId == StatusType.Pending && c.MediatorId != userId && !c.CaseReviews.Any(r => r.MediatorId == userId))
				.Select(c => new ReviewCaseTaskDetailsDto
				{
					Id = c.Id,
					Title = c.Title,
					NeededMoneyAmount = c.NeededMoneyAmount,
					DateRequested = c.DateRequested,
					Story = c.Story,
					Period = c.PeriodId.ToEnumString(),
					Mediator = new CaseMediatorDto
					{
						Id = c.MediatorId,
						Name = c.Mediator.Name,
						ImageUrl = Paths.ProfilePicture(c.MediatorId)
					},
					Reviews = c.CaseReviews.Select(r => new DTOs.Response.ReviewDto
					{
						Name = r.Mediator.Name,
						IsWorthy = r.IsWorthy,
						DateReviewed = r.DateReviewed,
						ImageUrl = Paths.ProfilePicture(r.MediatorId)
					})
				})
				.FirstOrDefaultAsync();
		}
	}
}
