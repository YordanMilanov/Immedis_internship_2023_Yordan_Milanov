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
        }
    }
}
