using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace GraduationProjectAPI.Models.Location
{
	public class GeoLocation
	{
		public int Id { get; set; }

		[Required]
		public Point Location { get; set; }

		[Required, MaxLength(4000)]
		public string Details { get; set; }
	}
}
