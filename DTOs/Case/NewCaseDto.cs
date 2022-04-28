using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Utilities.CustomAttributes;
using GraduationProjectAPI.Utilities.General;
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

		[Required, ImageFile(MaxSize = 1024 * 1024)]
		public IFormFile NationalIdImage { get; set; }

		[Required]
		public GenderType? GenderId { get; set; }

		[Required]
		public SocialStatusType? SocialStatusId { get; set; }

		// Case itselt

		[Required, MaxLength(250)]
		public string Title { get; set; }

		[Required, MaxLength(4000)]
		public string Story { get; set; }

		[Range(1, int.MaxValue)]
		public int NeededMoneyAmount { get; set; }

		public DateTime PaymentDate { get; set; }

		public byte Adults { get; set; }

		public byte Children { get; set; }

		[Required]
		public RelationshipType? RelationshipId { get; set; }

		[Required]
		public GeoLocationDto GeoLocation { get; set; }

		[Required, MaxLength(4000)]
		public string Address { get; set; }

		[Range(1, int.MaxValue)]
		public int RegionId { get; set; }

		[Range(1, byte.MaxValue)]
		public byte CategoryId { get; set; }

		[Required]
		public PeriodType? PeriodId { get; set; }

		[Required]
		public PriorityType PriorityId { get; set; }

		[ImageCollection(MaxSize = 1024 * 1024)]
		public IFormFileCollection OptionalImages { get; set; }

		public Models.Case ToCase()
		{
			return new Models.Case
			{
				Name = Name,
				PhoneNumber = PhoneNumber,
				BirthDate = BirthDate,
				NationalId = NationalId,
				Title = Title,
				Story = Story,
				NeededMoneyAmount = NeededMoneyAmount,
				PaymentDate = PaymentDate,
				Adults = Adults,
				Children = Children,
				Address = Address,
				RegionId = RegionId,
				CategoryId = CategoryId,
				GenderId = (byte)GenderId,
				SocialStatusId = (byte)SocialStatusId,
				RelationshipId = (byte)RelationshipId,
				PeriodId = (byte)PeriodId,
				PriorityId = (byte)PriorityId,
				GeoLocation = GeoLocation.ToGeoLocation(),
				NationalIdImage = FormFileHandler.ConvertToBytes(NationalIdImage),
				Images = (ICollection<Image>)OptionalImages.Select(i => new Image(FormFileHandler.ConvertToBytes(i)))
			};
		}
	}
}
