using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProjectAPI.Models
{
	public class GeoLocation
	{
		public int Id { get; set; }

		[Column(TypeName = "decimal(9,6)")]
		public decimal Longitude { get; set; }

		[Column(TypeName = "decimal(8,6)")]
		public decimal Latitude { get; set; }
	}
}
