using System.ComponentModel.DataAnnotations;

namespace GraduationProjectAPI.DTOs
{
	public class GeoLocationDTO
	{
		[Range(-180, 180)]
		public decimal Longitude { get; set; }

		[Range(-90, 90)]
		public decimal Latitude { get; set; }
	}
}
