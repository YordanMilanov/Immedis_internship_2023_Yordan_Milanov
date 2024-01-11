using AutoMapper;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Web.ViewModels.BaseViewModel;

namespace HCMS.Web.AutoMapperProfiles
{
    public class PageQueryMappingProfile : Profile
    {
        public PageQueryMappingProfile()
        {
            CreateMap<QueryDto, PageQueryModel>().ReverseMap();
        }
    }
}
