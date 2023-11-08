using AutoMapper;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.User;

namespace HCMS.Web.Api.AutoMapperProfiles
{
    public class QueryPageMappingProfile : Profile
    {
        public QueryPageMappingProfile()
        { 
            CreateMap<QueryDto, QueryParameterClass>().ReverseMap();

            CreateMap<QueryDto, QueryDtoResult<UserViewDto>>()
                .ForMember(dest => dest.ItemsPerPage, opt => opt.MapFrom(src => src.ItemsPerPage))
                .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
                .ForMember(dest => dest.SearchString, opt => opt.MapFrom(src => src.SearchString))
                .ForMember(dest => dest.OrderPageEnum, opt => opt.MapFrom(src => src.OrderPageEnum))
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.TotalItems, opt => opt.Ignore())
                .ReverseMap();

            //For Users
            CreateMap<QueryPageWrapClass<User>, QueryDtoResult<UserViewDto>>()
                       .ForMember(dest => dest.ItemsPerPage, opt => opt.Ignore())
                       .ForMember(dest => dest.CurrentPage, opt => opt.Ignore())
                       .ForMember(dest => dest.SearchString, opt => opt.Ignore())
                       .ForMember(dest => dest.OrderPageEnum, opt => opt.Ignore())
                       .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                       .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
                       .ReverseMap();
        }
    }
}
