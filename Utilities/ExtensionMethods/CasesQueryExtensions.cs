using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.DTOs.Response.Cases;
using GraduationProjectAPI.Models;
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
					Age = (short)(DateTime.Now - c.DateRequested).TotalDays,
					FundRaised = 4000,
					TotalNeeded = c.NeededMoneyAmount,
					NumberOfContributer = 32,
					ImageUrl = c.Images.Select(i => Paths.CaseImage(i.Id)).FirstOrDefault()
				}).ToArrayAsync();
		}

		public static async Task<CaseInfoDto> SelectCaseInfoDtoAsync(this IQueryable<Case> query, int id)
		{
			return await query.Select(c =>
				new CaseInfoDto
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
					History = new[]
					{
						new PreviousPaymentElementDto
						{
							Name = "Dummy Name",
							Amount = 1000,
							Datetime = DateTime.Now,
							ImageUrl = Paths.ProfilePicture(c.MediatorId)
						},
						new PreviousPaymentElementDto
						{
							Name = "Another Dummy Name",
							Amount = 1200,
							Datetime = DateTime.Now,
							ImageUrl = Paths.ProfilePicture(c.MediatorId)
						}
					}
				})
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public static async Task<IEnumerable<string>> SelectCaseImagesUrlsAsync(this IQueryable<Case> query, int id)
		{
			return await query.Where(c => c.Id == id)
				.Select(c => c.Images.Select(i => Paths.CaseImage(i.Id)))
				.FirstOrDefaultAsync();
		}
	}
}
