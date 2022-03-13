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
			CreateMap<GeoLocationDto, GeoLocation>().ReverseMap();
			CreateMap<Governorate, GovernorateDto>();
			CreateMap<City, CityDto>();
			CreateMap<Region, RegionDto>();
		}
	}
}
