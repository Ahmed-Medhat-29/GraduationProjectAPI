using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProjectAPI.Models.Location
{
	public class GeoLocation
	{
		public int Id { get; set; }

		[Column(TypeName = "decimal(9,6)")]
		public double Longitude { get; set; }

		[Column(TypeName = "decimal(8,6)")]
		public double Latitude { get; set; }

		[Required, MaxLength(4000)]
		public string Details { get; set; }
	}
}
