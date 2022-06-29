using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GraduationProjectAPI.Models
{
	public class Admin
	{
		public int Id { get; set; }

		[Required, MaxLength(250)]
		public string Name { get; set; }

		[Required, MaxLength(11), Column(TypeName = "char")]
		public string PhoneNumber { get; set; }

		[Required, MaxLength(14), Column(TypeName = "char")]
		public string NationalId { get; set; }

		[Required, MaxLength(1000)]
		public string Email { get; set; }

		[Required, MaxLength(4000)]
		public string PasswordHash { get; set; }

		[MaxLength(4000), Column(TypeName = "varchar")]
		public string FirebaseToken { get; set; }

		[Required]
		public byte[] ProfileImage { get; set; }

		[Required]
		public byte[] NationalIdImage { get; set; }

		[Column(TypeName = "datetime2(0)")]
		public DateTime DateRegistered { get; private set; } = DateTime.Now;

		public Gender Gender { get; set; }
		public GenderType GenderId { get; set; }
	}
}
