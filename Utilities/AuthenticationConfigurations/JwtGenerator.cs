﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GraduationProjectAPI.Utilities.AuthenticationConfigurations
{
	public class JwtGenerator : IAuthenticationTokenGenerator
	{
		private readonly IConfiguration _config;
		public string Token { get; private set; }

		public JwtGenerator(IConfiguration config)
		{
			_config = config;
		}

		public string Generate(string id, IEnumerable<KeyValuePair<string, string>> data)
		{
			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:Key"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var durationInMinutes = double.Parse(_config["JWT:DurationInMinutes"]);

			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
			claims.AddRange(data.Select(d => new Claim(d.Key, d.Value)));

			var securityToken = new JwtSecurityToken(
				issuer: _config["JWT:Issuer"],
				audience: _config["JWT:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(durationInMinutes),
				signingCredentials: credentials);

			Token = new JwtSecurityTokenHandler().WriteToken(securityToken);
			return Token;
		}
	}
}