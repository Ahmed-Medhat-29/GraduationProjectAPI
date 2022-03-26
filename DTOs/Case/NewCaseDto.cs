using System;
using System.ComponentModel.DataAnnotations;
using GraduationProjectAPI.Utilities.CustomAttributes;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.DTOs.Case
{
	public class NewCaseDto
	{
		// Responsipility Holder

		[Required, MaxLength(250)]
		public string Name { get; set; }

		[Required, MaxLength(11), MinLength(11), RegularExpression("^[0-9]+$", ErrorMessage = "Phone number must be only numbers")]
		public string PhoneNumber { get; set; }

		public DateTime BirthDate { get; set; }

		[Required, MaxLength(14), MinLength(14), RegularExpression("^[0-9]+$", ErrorMessage = "National id must be only numbers")]
		public string NationalId { get; set; }

		[Required, ImageFile]
		public IFormFile NationalIdImage { get; set; }

		[Range(1, 2)]
		public byte GenderId { get; set; }

		[Range(1, 4)]
		public byte SocialStatusId { get; set; }

		// Case itselt

		[Required, MaxLength(250)]
		public string Title { get; set; }

		[Required, MaxLength(4000)]
		public string Story { get; set; }

		[Range(1, int.MaxValue)]
		public int NeededMoneyAmount { get; set; }

		public DateTime? DateLimit { get; set; }

		public byte Adults { get; set; }

		public byte Children { get; set; }

		[Range(1, byte.MaxValue)]
		public byte RelationshipId { get; set; }

		[Required]
		public GeoLocationDto GeoLocation { get; set; }

		[Required, MaxLength(4000)]
		public string Address { get; set; }

		[Range(1, int.MaxValue)]
		public int RegionId { get; set; }

		[Range(1, byte.MaxValue)]
		public byte CategoryId { get; set; }

		[Range(1, byte.MaxValue)]
		public byte PriorityId { get; set; }

		[ImageCollection(MaxSize = 1024 * 1024)]
		public IFormFileCollection OptionalImages { get; set; }
	}
}
