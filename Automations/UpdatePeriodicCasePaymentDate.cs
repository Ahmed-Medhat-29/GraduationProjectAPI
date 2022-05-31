using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GraduationProjectAPI.Automations
{
	public class UpdatePeriodicCasePaymentDate : BackgroundService
	{
		private IServiceScopeFactory _scopeFactory;

		public UpdatePeriodicCasePaymentDate(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				if (DateTime.Now.Hour == 0)
				{
					using var scope = _scopeFactory.CreateScope();
					var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
					var cases = await GetPeriodicCasesAsync(context);

					context.AttachRange(cases);
					foreach (var @case in cases)
					{
						while (@case.PaymentDate < DateTime.Now)
						{
							@case.PaymentDate = @case.PaymentDate.AddMonths(1);
							@case.CurrentRound++;
						}
					}

					await context.SaveChangesAsync();
					context.Dispose();
				}

				await Task.Delay(1000 * 60 * 60, stoppingToken);
			}
		}

		private async Task<IEnumerable<Case>> GetPeriodicCasesAsync(ApplicationDbContext context)
		{
			return await context.Cases
						.Where(c => c.StatusId == Enums.StatusType.Accepted &&
									c.PaymentDate < DateTime.Now &&
									c.PeriodId == Enums.PeriodType.Monthly)
						.Select(c => new Case
						{
							Id = c.Id,
							PaymentDate = c.PaymentDate,
							CurrentRound = c.CurrentRound
						}).ToArrayAsync();
		}
	}
}
