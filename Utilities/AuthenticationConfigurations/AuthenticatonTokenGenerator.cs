using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GraduationProjectAPI.Utilities.AuthenticationConfigurations
{
	public class AuthenticatonTokenGenerator : IAuthenticationTokenGenerator
	{
		private readonly IConfiguration _config;
		public string Token { get; private set; }

		public AuthenticatonTokenGenerator(IConfiguration config)
		{
			_config = config;
		}

		public string Generate(string id, string imei, string firebaseToken)
		{
			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:Key"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var durationInMinutes = double.Parse(_config["JWT:DurationInMinutes"]);

			var securityToken = new JwtSecurityToken(
				issuer: _config["JWT:Issuer"],
				audience: _config["JWT:Audience"],
				claims: new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, id),
					new Claim("IMEI", imei),
					new Claim("FirebaseToken", firebaseToken)
				},
				expires: DateTime.Now.AddMinutes(durationInMinutes),
				signingCredentials: credentials);

			Token = new JwtSecurityTokenHandler().WriteToken(securityToken);
			return Token;
		}
	}
}
