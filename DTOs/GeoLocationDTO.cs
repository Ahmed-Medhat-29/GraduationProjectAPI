using System.ComponentModel.DataAnnotations;
using GraduationProjectAPI.Models.Location;

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

		public GeoLocationDto()
		{

		}

		public GeoLocationDto(GeoLocation geoLocation)
		{
			Longitude = geoLocation.Longitude;
			Latitude = geoLocation.Latitude;
			Details = geoLocation.Details;
		}

		public GeoLocation ToGeoLocation()
		{
			return new GeoLocation
			{
				Longitude = Longitude,
				Latitude = Latitude,
				Details = Details
			};
		}
	}
}
