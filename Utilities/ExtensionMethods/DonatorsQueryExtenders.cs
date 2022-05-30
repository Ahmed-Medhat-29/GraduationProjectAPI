using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.DTOs.Response.Donators;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Utilities.ExtensionMethods
{
	public static class DonatorsQueryExtenders
	{
		public static async Task<DonatorDetails> SelectDonatorDetailsDtoAsync(this IQueryable<Donator> query, IAuthenticationTokenGenerator tokenGenerator, string phoneNumber)
		{
			return await query
				.Select(d => new DonatorDetails
				{
					Name = d.Name,
					PhoneNumber = d.PhoneNumber,
					JwtToken = tokenGenerator.Generate(d.Id.ToString(), Roles.Donator),
					FirebaseToken = d.FirebaseToken,
					Locale = d.LocaleId.ToString()
				})
				.FirstAsync(d => d.PhoneNumber == phoneNumber);
		}
	}
}
