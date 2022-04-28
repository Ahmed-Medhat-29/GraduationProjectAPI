using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GraduationProjectAPI.Models
{
	public class Notification
	{
		public int Id { get; set; }

		[Required, MaxLength(250)]
		public string Title { get; set; }

		[Required, MaxLength(4000)]
		public string Body { get; set; }

		public bool IsRead { get; set; }

		[Column(TypeName = "datetime2(0)")]
		public DateTime DateTime { get; private set; } = DateTime.Now;

		[Required, Column(TypeName = "varchar(4000)")]
		public string ImageUrl { get; set; }

		public int TaskId { get; set; }

		public NotificationType Type { get; set; }
		public byte TypeId { get; set; }

		public Mediator Mediator { get; set; }
		public int MediatorId { get; set; }
	}
}
