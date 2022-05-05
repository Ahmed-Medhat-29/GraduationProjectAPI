using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduationProjectAPI.Enums;

namespace GraduationProjectAPI.Models.Shared
{
	public class Locale
	{
		public LocaleType Id { get; set; }

		[Required, MaxLength(10), Column(TypeName = "varchar")]
		public string Name { get; set; }
	}
}
