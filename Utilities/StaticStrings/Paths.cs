using System.Text;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Utilities.StaticStrings
{
	public static class Paths
	{
		private static StringBuilder _common;
		private const string _profilePicture = "/api/mediators/profile-image/";
		private const string _nationalIdImage = "/api/mediators/nationalid-image/";
		private const string _caseImage = "/api/cases/images/";

		private static StringBuilder InitCommon(HttpRequest request)
		{
			if (_common == null)
				_common = new StringBuilder(request.Scheme).Append("://").Append(request.Host).Append(request.PathBase.ToString());

			return _common;
		}

		public static string ProfilePicture(HttpRequest request, int id)
		{
			return InitCommon(request).Append(_profilePicture).Append(id).ToString();
		}

		public static string NationalIdImage(HttpRequest request, int id)
		{
			return InitCommon(request).Append(_nationalIdImage).Append(id).ToString();
		}

		public static string CaseImage(HttpRequest request, int id)
		{
			return InitCommon(request).Append(_caseImage).Append(id).ToString();
		}
	}
}
