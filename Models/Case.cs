using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Threading.Tasks;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Models.Location;
using GraduationProjectAPI.Models.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Image = GraduationProjectAPI.Models.CaseProperties.Image;

namespace GraduationProjectAPI.Models
{
	public class Case
	{
		public int Id { get; set; }

		[Required, MaxLength(250)]
		public string Name { get; set; }

		[Required, MaxLength(11), Column(TypeName = "varchar")]
		public string PhoneNumber { get; set; }

		[Required, MaxLength(14), Column(TypeName = "varchar")]
		public string NationalId { get; set; }

		[Column(TypeName = "date")]
		public DateTime BirthDate { get; set; }

		public byte Adults { get; set; }

		public byte Children { get; set; }

		public int NeededMoneyAmount { get; set; }

		[Column(TypeName = "date")]
		public DateTime PaymentDate { get; set; }

		[Column(TypeName = "date")]
		public DateTime DateRequested { get; private set; } = DateTime.Now;

		public byte[] NationalIdImage { get; set; }

		[MaxLength(4000)]
		public string Address { get; set; }

		[Required, MaxLength(250)]
		public string Title { get; set; }

		[Required, MaxLength(4000)]
		public string Story { get; set; }

		public Period Period { get; set; }
		public byte PeriodId { get; set; }

		public Mediator Mediator { get; set; }
		public int MediatorId { get; set; }

		public Category Category { get; set; }
		public byte CategoryId { get; set; }

		public Relationship Relationship { get; set; }
		public byte RelationshipId { get; set; }

		public Priority Priority { get; set; }
		public byte PriorityId { get; set; }

		public Gender Gender { get; set; }
		public byte GenderId { get; set; }

		public GeoLocation GeoLocation { get; set; }
		public int GeoLocationId { get; set; }

		public SocialStatus SocialStatus { get; set; }
		public byte SocialStatusId { get; set; }

		public Region Region { get; set; }
		public int RegionId { get; set; }

		public Status Status { get; set; }
		public byte StatusId { get; set; }

		public ICollection<Image> Images { get; set; }

		public async Task SetNationalIdImageAsync(IFormFile nationalIdImage)
		{
			using (var stream = new MemoryStream())
			{
				await nationalIdImage.CopyToAsync(stream);
				NationalIdImage = stream.ToArray();
			}
		}

		public void AddOptionalImages(IEnumerable<IFormFile> images)
		{
			if (Images == null)
				Images = new List<Image>();

			foreach (var image in images)
				Images.Add(new Image(image));
		}
	}
}
