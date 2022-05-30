using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProjectAPI.Models.CaseProperties
{
	public class CasePayment
	{
		public int Id { get; set; }

		public int AdminId { get; set; }

		public Mediator Mediator { get; set; }
		public int MediatorId { get; set; }

		public Case Case { get; set; }
		public int CaseId { get; set; }

		public int Amount { get; set; }

		[MaxLength(4000)]
		public string Details { get; set; }

		public int? RoundNnumber { get; set; }

		[Column(TypeName = "datetime2(0)")]
		public DateTime DateSubmitted { get; set; }

		[Column(TypeName = "datetime2(0)")]
		public DateTime? DateDelivered { get; set; }
	}
}
