using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GraduationProjectAPI.Models
{
	public class Donator
	{
		public int Id { get; set; }

		[Required, MaxLength(250)]
		public string Name { get; set; }

		[Required, MaxLength(11), Column(TypeName = "char")]
		public string PhoneNumber { get; set; }

		[MaxLength(4000), Column(TypeName = "varchar")]
		public string FirebaseToken { get; set; }

		[Column(TypeName = "datetime2(0)")]
		public DateTime DateRegistered { get; private set; } = DateTime.Now;

		public Donator()
		{

		}

		public Donator(int id)
		{
			Id = id;
		}
	}
}
