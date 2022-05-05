using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduationProjectAPI.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GraduationProjectAPI.Models.Shared
{
	public class Gender
	{
		public GenderType Id { get; set; }

		[Required, MaxLength(10), Column(TypeName = "varchar")]
		public string Name { get; set; }
	}
}
