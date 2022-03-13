namespace GraduationProjectAPI.Utilities.CustomApiResponses
{
	public interface IApiResponse
	{
		public byte Status { get; }
		public string Message { get; }
	}
}
