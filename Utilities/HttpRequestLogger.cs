using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Utilities
{
	public static class HttpRequestLogger
	{
		public static void Log(HttpRequest request)
		{
			if (request.HasFormContentType)
			{
				var strBuilder = new StringBuilder();
				strBuilder.AppendLine("ContentType " + request.ContentType.ToString());
				using (var enumerator = request.Form.GetEnumerator())
					while (enumerator.MoveNext())
					{
						var item = enumerator.Current;
						strBuilder.AppendLine("Key: " + item.Key + " | Value: " + item.Value);
					}

				File.WriteAllText("log.txt", strBuilder.ToString());
			}
		}
	}
}
