using AutoMapper;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.DTOs.Mediator;
using GraduationProjectAPI.Models;

namespace GraduationProjectAPI.Utilities
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<MediatorRegister, Mediator>().ReverseMap();
			CreateMap<Mediator, MediatorProfile>();
			CreateMap<GeoLocationDTO, GeoLocation>().ReverseMap();
			CreateMap<Governorate, GovernorateDTO>();
			CreateMap<City, CityDTO>();
			CreateMap<Region, RegionDTO>();
		}
	}
}
