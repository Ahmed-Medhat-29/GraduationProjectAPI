using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GraduationProjectAPI.Utilities.NotificationsManagement
{
	public class NotificationHandler
	{
		private readonly Notification _notification;

		public NotificationHandler(string title, string body)
		{
			_notification = new Notification(title, body);
		}

		public async Task SendAsync(string token)
		{
			var client = CreateHttpClient();
			var content = InitContent(token);
			await MakeRequestAsync(client, content);
			client.Dispose();
		}

		public async Task SendAsync(string[] tokens)
		{
			var client = CreateHttpClient();
			foreach (var token in tokens)
			{
				var content = InitContent(token);
				await MakeRequestAsync(client, content);
			}
			client.Dispose();
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
			HttpResponseMessage response = null;
			do
			{
				response = await client.PostAsJsonAsync("https://fcm.googleapis.com/fcm/send", content.Value);
			} while (response != null && response.StatusCode != System.Net.HttpStatusCode.OK);
			response.Dispose();
			//var responseMessage = await response.Content.ReadAsStringAsync();
			//System.Console.WriteLine(responseMessage);
		}
	}
}
