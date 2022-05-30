using System.Security.Claims;

namespace GraduationProjectAPI.Utilities.General
{
	public static class UserHandler
	{
		private static int _id;

		public static int GetId(ClaimsPrincipal user)
		{
			return _id != 0 ? _id : _id = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
		}
	}
}
