using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GraduationProjectAPI.Models.Location
{
	public class Region
	{
		public int Id { get; set; }

		[Required, MaxLength(250), Column(TypeName = "varchar")]
		public string Name { get; set; }

		public City City { get; set; }
		public int CityId { get; set; }
	}
}
