using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GraduationProjectAPI.Utilities.CustomApiResponses
{
	public class BadRequest : IActionResult, IApiResponse
	{
		public byte Status { get; } = 0;
		public string Message { get; } = "Error";
		public IDictionary<string, IEnumerable<string>> Errors { get; private set; }

		public BadRequest(string errorMessage)
		{
			Message = errorMessage;
		}

		public BadRequest(ModelStateDictionary modelState)
		{
			var invalidModels = modelState.Where(m => m.Value.Errors.Count > 0);
			if (modelState.ErrorCount == 1 && string.IsNullOrWhiteSpace(invalidModels.First().Key))
				Message = invalidModels.First().Value.Errors.Select(e => e.ErrorMessage).First();
			else
			{
				Errors = new Dictionary<string, IEnumerable<string>>();
				foreach (var model in invalidModels)
					Errors.Add(model.Key, model.Value.Errors.Select(e => e.ErrorMessage));

			}
		}

		public BadRequest(IDictionary<string, IEnumerable<string>> errors)
		{
			Errors = errors;
		}

		public BadRequest(KeyValuePair<string, IEnumerable<string>> error)
		{
			Errors = new Dictionary<string, IEnumerable<string>>();
			Errors.Add(error);
		}

		public BadRequest(string propertyName, string errorMessage)
		{
			Errors = new Dictionary<string, IEnumerable<string>>();
			Errors.Add(new KeyValuePair<string, IEnumerable<string>>(propertyName, new[] { errorMessage }));
		}

		public async Task ExecuteResultAsync(ActionContext context)
		{
			var objectResult = new ObjectResult(this)
			{
				StatusCode = StatusCodes.Status400BadRequest
			};

			await objectResult.ExecuteResultAsync(context);
		}
	}
}
