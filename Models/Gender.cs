using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GraduationProjectAPI.Models
{
	public class Gender
	{
		public byte Id { get; set; }

		[Required, MaxLength(10), Column(TypeName = "varchar")]
		public string Name { get; set; }
	}
}
