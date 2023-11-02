using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Services.ServiceModels.Education;
using HCMS.Web.ViewModels.Education;

namespace HCMS.Web.AutoMapperProfiles
{
    public class EducationMappingProfile : Profile
    {
        public EducationMappingProfile()
        {
            CreateMap<EducationDto, EducationFormModel>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.GetCountry()))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Location.GetState()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location.GetAddress()));

            CreateMap<EducationFormModel, EducationDto>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                    new LocationStruct(src.Address, src.State, src.Country)));


            CreateMap<EducationDto, EducationViewModel>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location.GetAddress()))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Location.GetState()))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.GetCountry()));
        }
    }
}
