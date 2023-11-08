using AutoMapper;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.User;
using HCMS.Web.ViewModels.BaseViewModel;
using HCMS.Web.ViewModels.User;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace HCMS.Web.AutoMapperProfiles
{
    public class PageQueryMappingProfile : Profile
    {
        public PageQueryMappingProfile()
        {
            CreateMap<QueryDto, PageQueryModel>().ReverseMap();
            

            CreateMap<ResultQueryModel<UserViewModel>, QueryDtoResult<UserViewDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems));

            CreateMap<QueryDtoResult<UserViewDto>, ResultQueryModel<UserViewModel>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems));


        }
    }
}
