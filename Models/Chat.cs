using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GraduationProjectAPI.Models
{
	public class Chat
	{
		public int Id { get; set; }

		public int ChatId { get; set; }

		[Required, MaxLength(4000)]
		public string Message { get; set; }

		[Column(TypeName = "datetime2(2)")]
		public DateTime DateTime { get; set; } = DateTime.Now;

		public MessageType MessageType { get; set; }
		public byte MessageTypeId { get; set; }

		public Mediator Mediator { get; set; }
		public int MediatorId { get; set; }
	}
}
