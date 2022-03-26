using AutoMapper;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.DTOs.Case;
using GraduationProjectAPI.DTOs.Mediator;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Models.Location;
using GraduationProjectAPI.Models.Reviews;

namespace GraduationProjectAPI.Utilities
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<GeoLocationDto, GeoLocation>();
			CreateMap<RegisterDto, Mediator>();
			CreateMap<NewCaseDto, Case>().ForMember(d => d.NationalIdImage, act => act.Ignore());
			CreateMap<ReviewDto, CaseReview>();
		}
	}
}
