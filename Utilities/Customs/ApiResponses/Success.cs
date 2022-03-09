using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProjectAPI.Utilities.Customs.ApiResponses
{
	public class Success : IActionResult
	{
		public byte Status { get; } = 1;
		public string Message { get; } = "Success";
		public object Data { get; private set; }

		public Success()
		{

		}

		public Success(object obj)
		{
			Data = obj;
		}

		public async Task ExecuteResultAsync(ActionContext context)
		{
			var objectResult = new ObjectResult(this)
			{
				StatusCode = StatusCodes.Status200OK
			};

			await objectResult.ExecuteResultAsync(context);
		}
	}
}
