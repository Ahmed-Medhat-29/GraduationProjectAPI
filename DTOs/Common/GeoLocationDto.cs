using System.ComponentModel.DataAnnotations;
using GraduationProjectAPI.Models.Location;
using NetTopologySuite.Geometries;

namespace GraduationProjectAPI.DTOs.Common
{
	public class GeoLocationDto
	{
		[Range(-180, 180)]
		public double Longitude { get; set; }

		[Range(-90, 90)]
		public double Latitude { get; set; }

		[Required, MaxLength(4000)]
		public string Details { get; set; }

		public GeoLocation ToGeoLocation()
		{
			return new GeoLocation
			{
				Location = new Point(Longitude, Latitude) { SRID = 4326 },
				Details = Details
			};
		}
	}
}
