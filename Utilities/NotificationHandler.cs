using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GraduationProjectAPI.DTOs;

namespace GraduationProjectAPI.Utilities
{
	public class NotificationHandler
	{
		private readonly NotificationDto _notification;

		public NotificationHandler(string title, string body)
		{
			_notification = new NotificationDto(title, body);
		}

		public async Task SendAsync(string token)
		{
			var content = InitContent(token);
			using var client = CreateHttpClient();
			await MakeRequestAsync(client, content);
		}

		public async Task SendAsync(IEnumerable<string> tokens)
		{
			using var client = CreateHttpClient();
			foreach (var token in tokens)
			{
				var content = InitContent(token);
				await MakeRequestAsync(client, content);
			}
		}

		private static HttpClient CreateHttpClient()
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=AAAACvfdU-8:APA91bHkpbTcB5Bxv5SekyEm_9k5iEZBMgqTaspodSXCdgq3wIG-vEW63QgjYpF1NMHWR2oqIwWXb1FppQdPlJNhZsj4QGxFCNcy6oLTqjktQip0Lx-3kCtYp7cAxE1O9xhkMy6HJf63");
			return client;
		}

		private JsonContent InitContent(string token)
		{
			return JsonContent.Create(new
			{
				To = token,
				Data = new
				{
					Notification = _notification
				}
			});
		}

		private static async Task MakeRequestAsync(HttpClient client, JsonContent content)
		{
			var response = await client.PostAsJsonAsync("https://fcm.googleapis.com/fcm/send", content.Value);
			response.Dispose();
			//var responseMessage = await response.Content.ReadAsStringAsync();
			//System.Console.WriteLine(responseMessage);
		}
	}
}
