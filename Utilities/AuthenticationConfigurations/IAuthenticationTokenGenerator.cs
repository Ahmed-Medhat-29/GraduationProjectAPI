using System.Collections.Generic;

namespace GraduationProjectAPI.Utilities.AuthenticationConfigurations
{
	public interface IAuthenticationTokenGenerator
	{
		string Token { get; }
		string Generate(string id);
		string Generate(string id, IDictionary<string, string> data);
	}
}
