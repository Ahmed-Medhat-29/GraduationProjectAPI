using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.DTOs.Response;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.General;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Utilities.ExtensionMethods
{
	public static class NotificationsQueryExtenders
	{
		public static async Task<NotificationDto[]> SelectNotificationsAsync(this IQueryable<Notification> query, int id, int page)
		{
			return await query.Where(n => n.MediatorId == id)
				.OrderBy(n => n.DateTime)
				.Select(n => new NotificationDto
				{
					Id = n.Id,
					Title = n.Title,
					Body = n.Body,
					IsRead = n.IsRead,
					TaskId = n.TaskId,
					DateTime = n.DateTime,
					ImageUrl = n.ImageUrl,
					Type = n.TypeId.ToString()
				})
				.Skip(Pagination.MaxPageSize * (page - 1))
				.Take(Pagination.MaxPageSize)
				.ToArrayAsync();
		}
	}
}
