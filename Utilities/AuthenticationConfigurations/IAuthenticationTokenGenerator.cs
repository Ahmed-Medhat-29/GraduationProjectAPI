using System.Collections.Generic;

namespace GraduationProjectAPI.Utilities.AuthenticationConfigurations
{
	public interface IAuthenticationTokenGenerator
	{
		string Token { get; }
		string Generate(string id, string role);
		string Generate(string id, string role, IDictionary<string, string> data);
	}
}
