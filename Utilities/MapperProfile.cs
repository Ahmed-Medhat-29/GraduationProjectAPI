using System.Linq;
using AutoMapper;
using GraduationProjectAPI.DTOs;
using GraduationProjectAPI.DTOs.Case;
using GraduationProjectAPI.DTOs.Mediator;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Models.Location;
using GraduationProjectAPI.Models.Reviews;

namespace GraduationProjectAPI.Utilities
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<GeoLocationDto, GeoLocation>();

			CreateMap<RegisterDto, Mediator>()
				.ForMember(d => d.NationalIdImage, act => act.MapFrom(src => FormFileHandler.ConvertToBytes(src.NationalIdImage)))
				.ForMember(d => d.ProfileImage, act => act.MapFrom(src => FormFileHandler.ConvertToBytes(src.ProfileImage)));

			CreateMap<NewCaseDto, Case>()
				.ForMember(d => d.NationalIdImage, act => act.MapFrom(src => FormFileHandler.ConvertToBytes(src.NationalIdImage)))
				.ForMember(d => d.Images, act => act.MapFrom(src => src.OptionalImages.Select(i => new Image(FormFileHandler.ConvertToBytes(i)))));

			CreateMap<ReviewDto, MediatorReview>();
			CreateMap<ReviewDto, CaseReview>();
		}
	}
}
