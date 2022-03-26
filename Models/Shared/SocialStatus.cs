using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GraduationProjectAPI.Models.Shared
{
	public class SocialStatus
	{
		public byte Id { get; set; }

		[Required, MaxLength(50), Column(TypeName = "varchar")]
		public string Name { get; set; }
	}
}
