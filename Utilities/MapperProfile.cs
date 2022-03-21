using AutoMapper;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.DTOs.Case;
using GraduationProjectAPI.DTOs.Mediator;
using GraduationProjectAPI.Models;

namespace GraduationProjectAPI.Utilities
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<MediatorRegister, Mediator>().ForMember(d => d.NotificationToken, opt => opt.MapFrom(src => src.FirebaseToken));
			CreateMap<Mediator, MediatorProfile>();
			CreateMap<GeoLocationDto, GeoLocation>();
			CreateMap<CaseAddingDto, Case>().ForMember(d => d.NationalIdImage, act => act.Ignore());
			CreateMap<Governorate, GovernorateDto>();
			CreateMap<City, CityDto>();
			CreateMap<Region, RegionDto>();
		}
	}
}
