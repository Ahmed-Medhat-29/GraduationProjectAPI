using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProjectAPI.Models
{
	public class MessageType
	{
		public byte Id { get; set; }

		[Required, MaxLength(20), Column(TypeName = "varchar")]
		public string Name { get; set; }
	}
}
