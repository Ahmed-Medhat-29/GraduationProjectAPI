using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GraduationProjectAPI.Models
{
	public class Mediator
	{
		public int Id { get; set; }

		[Required, MaxLength(250)]
		public string Name { get; set; }

		[Required, MaxLength(11), Column(TypeName = "varchar")]
		public string PhoneNumber { get; set; }

		[Required, MaxLength(14), Column(TypeName = "varchar")]
		public string NationalId { get; set; }

		[MaxLength(250)]
		public string Job { get; set; }

		[MaxLength(4000)]
		public string Address { get; set; }

		[Column(TypeName = "date")]
		public DateTime? BirthDate { get; set; }

		[MaxLength(4000)]
		public string Bio { get; set; }

		[MaxLength(4000)]
		public string NationalIdImageName { get; set; }

		[MaxLength(4000)]
		public string ProfileImageName { get; set; }

		public Region Region { get; set; }
		public int? RegionId { get; set; }

		public GeoLocation GeoLocation { get; set; }
		public int GeoLocationId { get; set; }

		public Gender Gender { get; set; }
		public byte GenderId { get; set; }

		public SocialStatus SocialStatus { get; set; }
		public byte SocialStatusId { get; set; }

		public Status Status { get; set; }
		public byte StatusId { get; set; }

		public async Task SetNationalIdImageNameAsync(IFormFile nationalIdImage)
		{
			await new MediatorImagesHandler(this).SetMediatorNationalIdImageAsync(nationalIdImage);
		}

		public async Task SetProfileImageAsync(IFormFile profileImage)
		{
			await new MediatorImagesHandler(this).SetMediatorProfileImageAsync(profileImage);
		}
	}
}
