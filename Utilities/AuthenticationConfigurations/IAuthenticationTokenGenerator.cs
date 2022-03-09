namespace GraduationProjectAPI.Utilities.AuthenticationConfigurations
{
	public interface IAuthenticationTokenGenerator
	{
		string Token { get; }
		string Generate(string id, string imei, string firebaseToken);
	}
}
