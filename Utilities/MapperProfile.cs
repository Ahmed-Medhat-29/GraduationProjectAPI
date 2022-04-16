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
			CreateMap<RegisterDto, Mediator>()
				.ForMember(d => d.NationalIdImage, act => act.Ignore())
				.ForMember(d => d.ProfileImage, act => act.Ignore());

			CreateMap<GeoLocationDto, GeoLocation>();
			CreateMap<NewCaseDto, Case>().ForMember(d => d.NationalIdImage, act => act.Ignore());
			CreateMap<ReviewDto, MediatorReview>();
			CreateMap<ReviewDto, CaseReview>();
		}
	}
}
