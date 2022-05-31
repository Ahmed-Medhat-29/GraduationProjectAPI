using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Models.Location;
using GraduationProjectAPI.Models.Reviews;
using GraduationProjectAPI.Models.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GraduationProjectAPI.Models
{
	public class Case
	{
		public int Id { get; set; }

		[Required, MaxLength(250)]
		public string Name { get; set; }

		[Required, MaxLength(11), Column(TypeName = "char")]
		public string PhoneNumber { get; set; }

		[Required, MaxLength(14), Column(TypeName = "char")]
		public string NationalId { get; set; }

		[Column(TypeName = "date")]
		public DateTime BirthDate { get; set; }

		public byte Adults { get; set; }

		public byte Children { get; set; }

		public int NeededMoneyAmount { get; set; }

		[Column(TypeName = "date")]
		public DateTime PaymentDate { get; set; }

		public int? CurrentRound { get; set; }

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
		public PeriodType PeriodId { get; set; }

		public Mediator Mediator { get; set; }
		public int MediatorId { get; set; }

		public Category Category { get; set; }
		public int CategoryId { get; set; }

		public Relationship Relationship { get; set; }
		public RelationshipType RelationshipId { get; set; }

		public Priority Priority { get; set; }
		public PriorityType PriorityId { get; set; }

		public Gender Gender { get; set; }
		public GenderType GenderId { get; set; }

		public GeoLocation GeoLocation { get; set; }
		public int GeoLocationId { get; set; }

		public SocialStatus SocialStatus { get; set; }
		public SocialStatusType SocialStatusId { get; set; }

		public Region Region { get; set; }
		public int RegionId { get; set; }

		public Status Status { get; set; }
		public StatusType StatusId { get; set; }

		public ICollection<CaseReview> CaseReviews { get; set; }
		public ICollection<CasePayment> CasePayments { get; set; }
		public ICollection<Image> Images { get; set; }
	}
}
