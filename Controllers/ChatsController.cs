using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProjectAPI.Data;
using GraduationProjectAPI.DTOs.Request.Chats;
using GraduationProjectAPI.DTOs.Response.Chats;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.CustomApiResponses;
using GraduationProjectAPI.Utilities.General;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraduationProjectAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = Roles.Mediator)]
	public class ChatsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceScopeFactory _scopeFactory;

		public ChatsController(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
		{
			_context = context;
			_scopeFactory = scopeFactory;
		}

		[HttpGet]
		public async Task<IActionResult> Chat()
		{
			var chat = await _context.Chats
				.Where(c => c.MediatorId == GetUserId())
				.Select(c => new ChatResponseDto
				{
					Message = c.Message,
					DateTime = c.DateTime,
					Type = c.MessageTypeId.ToString()
				})
				.ToArrayAsync();

			return new Success(chat);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Message([FromForm] NewChatDto dto)
		{
			var chat = dto.ToChat(GetUserId(), Enums.MessageType.Sent);
			await _context.Chats.AddAsync(chat);
			await _context.SaveChangesAsync();
			return new Success();
		}

		// waiting for dashboard
		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<IActionResult> SendMessageTo([FromForm] int mediatorId, [FromForm] NewChatDto dto)
		{
			var chat = dto.ToChat(mediatorId, Enums.MessageType.Received);
			await _context.Chats.AddAsync(chat);
			await _context.SaveChangesAsync();
			_ = NewMesaageNotificationAsync(chat);
			return new Success();
		}

		// ************************ Private methods ************************

		private async Task NewMesaageNotificationAsync(Chat chat)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var firebaseToken = context.Mediators.Where(m => m.Id == chat.MediatorId)
				.Select(m => m.FirebaseToken)
				.FirstOrDefault();

			if (string.IsNullOrWhiteSpace(firebaseToken))
				return;

			var handler = new NotificationHandler(new ChatResponseDto(chat));
			await handler.SendAsync(firebaseToken);
		}

		private int GetUserId()
		{
			return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
		}
	}
}
