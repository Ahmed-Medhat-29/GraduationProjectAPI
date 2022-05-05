using System.Linq;
using System.Threading.Tasks;
using GraduationProjectAPI.DTOs.Common;
using GraduationProjectAPI.DTOs.Response.Mediators;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Utilities.AuthenticationConfigurations;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Utilities.ExtensionMethods
{
	public static class MediatorsQueryExtenders
	{
		public static async Task<SignInResponseDto> SelectSignInResponseDtoAsync(this IQueryable<Mediator> query, IAuthenticationTokenGenerator tokenGenerator, string phoneNumber)
		{
			return await query
				.Select(m => new SignInResponseDto
				{
					Name = m.Name,
					PhoneNumber = m.PhoneNumber,
					NationalId = m.NationalId,
					Job = m.Job,
					Address = m.Address,
					BirthDate = m.BirthDate,
					Bio = m.Bio,
					Completed = m.Completed,
					JwtToken = tokenGenerator.Generate(m.Id.ToString(), Roles.Mediator),
					FirebaseToken = m.FirebaseToken,
					Gender = m.GenderId.ToString(),
					Region = m.Region.Name,
					SocialStatus = m.SocialStatusId.ToString(),
					Locale = m.LocaleId.ToString(),
					Status = m.StatusId.ToString(),
					ProfileImageUrl = Paths.ProfilePicture(m.Id),
					NationalIdImageUrl = Paths.NationalIdImage(m.Id),
					GeoLocation = new GeoLocationDto
					{
						Longitude = m.GeoLocation.Location.Coordinate.X,
						Latitude = m.GeoLocation.Location.Coordinate.Y,
						Details = m.GeoLocation.Details
					}
				})
				.FirstAsync(m => m.PhoneNumber == phoneNumber);
		}

		public static async Task<ProfileDto> SelectProfileDtoAsync(this IQueryable<Mediator> query, int id)
		{
			return await query.Where(m => m.Id == id)
				.Select(m => new ProfileDto
				{
					Name = m.Name,
					Bio = m.Bio,
					SocialStatusId = m.SocialStatusId,
					ImageUrl = Paths.ProfilePicture(m.Id)
				})
				.FirstAsync();
		}

		public static async Task<byte[]> SelectProfileImageAsync(this IQueryable<Mediator> query, int id)
		{
			return await query.Where(m => m.Id == id)
				.Select(m => m.ProfileImage)
				.FirstOrDefaultAsync();
		}

		public static async Task<byte[]> SelectNationalIdImageAsync(this IQueryable<Mediator> query, int id)
		{
			return await query.Where(m => m.Id == id)
				.Select(m => m.NationalIdImage)
				.FirstOrDefaultAsync();
		}
	}
}
