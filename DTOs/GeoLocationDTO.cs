using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.DTOs
{
	public class GeoLocationDto
	{
		[Range(-180, 180)]
		public double Longitude { get; set; }

		[Range(-90, 90)]
		public double Latitude { get; set; }

		[Required, MaxLength(4000)]
		public string Details { get; set; }
	}
}
