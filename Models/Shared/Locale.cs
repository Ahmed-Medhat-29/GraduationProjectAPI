using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProjectAPI.Models.Shared
{
	public class Locale
	{
		public byte Id { get; set; }

		[Required, MaxLength(10), Column(TypeName = "varchar")]
		public string Name { get; set; }
	}
}
