using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Utilities.General
{
	public static class HttpRequestLogger
	{
		public static void Log(HttpRequest request)
		{
			var builder = new StringBuilder($"ContentType {request.ContentType}\n");
			if (request.HasFormContentType)
				using (var enumerator = request.Form.GetEnumerator())
					while (enumerator.MoveNext())
						builder.AppendLine($"Key: {enumerator.Current.Key} | Value: {enumerator.Current.Value}");

			if (!Directory.Exists("wwwroot"))
				Directory.CreateDirectory("wwwroot");

			File.WriteAllText(@"wwwroot\log.txt", builder.ToString());
		}
	}
}
