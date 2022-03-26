using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProjectAPI.Models.CaseProperties
{
	public class Priority
	{
		public byte Id { get; set; }

		[Required, MaxLength(50), Column(TypeName = "varchar")]
		public string Name { get; set; }
	}
}
