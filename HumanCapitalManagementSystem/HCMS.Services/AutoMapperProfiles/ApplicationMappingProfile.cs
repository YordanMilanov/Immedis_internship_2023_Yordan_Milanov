using AutoMapper;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Services.ServiceModels.Application;
using HCMS.Services.ServiceModels.BaseClasses;

namespace HCMS.Services.AutoMapperProfiles
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<ApplicationDto, Application>().ReverseMap();

            CreateMap<QueryPageWrapClass<Application>, QueryDtoResult<ApplicationDto>>()
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ReverseMap();


        }
    }
}
