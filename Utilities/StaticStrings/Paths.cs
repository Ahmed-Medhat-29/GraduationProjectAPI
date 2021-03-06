using System.Text;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Utilities.StaticStrings
{
	public static class Paths
	{
		private static string _baseURL;
		private const string _profilePicture = "/api/mediators/profile-image/";
		private const string _nationalIdImage = "/api/mediators/nationalid-image/";
		private const string _caseImage = "/api/cases/images/";
		private const string _categoryImage = "/api/category-image/";
		private const string _governorateImage = "/api/governorate-image/";

		public static void InitBaseURL(HttpRequest Request)
		{
			_baseURL = new StringBuilder(Request.Scheme).Append("://").Append(Request.Host).Append(Request.PathBase.ToString()).ToString();
		}

		public static string ProfilePicture(int id)
		{
			return new StringBuilder(_baseURL).Append(_profilePicture).Append(id).ToString();
		}

		public static string NationalIdImage(int id)
		{
			return new StringBuilder(_baseURL).Append(_nationalIdImage).Append(id).ToString();
		}

		public static string CaseImage(int id)
		{
			return new StringBuilder(_baseURL).Append(_caseImage).Append(id).ToString();
		}

		public static string CategoryImage(int id)
		{
			return new StringBuilder(_baseURL).Append(_categoryImage).Append(id).ToString();
		}

		public static string GovernorateImage(int id)
		{
			return new StringBuilder(_baseURL).Append(_governorateImage).Append(id).ToString();
		}
	}
}
