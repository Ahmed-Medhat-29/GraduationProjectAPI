using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.Models.CaseProperties
{
	public class Period
	{
		public byte Id { get; set; }

		[Required, MaxLength(50), Column(TypeName = "varchar")]
		public string Name { get; set; }
	}
}
