using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
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

		//[Required]
		public byte[] NationalIdImage { get; set; }

		public byte[] ProfileImage { get; set; }

		public GeoLocation GeoLocation { get; set; }
		public int GeoLocationId { get; set; }

		public Region Region { get; set; }
		public int? RegionId { get; set; }

		public Gender Gender { get; set; }
		public byte GenderId { get; set; }

		public SocialStatus SocialStatus { get; set; }
		public byte SocialStatusId { get; set; }

		public Status Status { get; set; }
		public byte StatusId { get; set; }

		public async Task SetNationalIdImageAsync(IFormFile nationalIdImage)
		{
			using (var stream = new MemoryStream())
			{
				await nationalIdImage.CopyToAsync(stream);
				NationalIdImage = stream.ToArray();
			}
		}

		public async Task SetProfileImageAsync(IFormFile profileImage)
		{
			using (var stream = new MemoryStream())
			{
				await profileImage.CopyToAsync(stream);
				ProfileImage = stream.ToArray();
			}
		}
	}
}
