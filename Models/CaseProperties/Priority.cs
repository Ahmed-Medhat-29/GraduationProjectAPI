﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduationProjectAPI.Enums;

namespace GraduationProjectAPI.Models.CaseProperties
{
	public class Priority
	{
		public PriorityType Id { get; set; }

		[Required, MaxLength(50), Column(TypeName = "varchar")]
		public string Name { get; set; }
	}
}
