using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Services.ServiceModels.Advert;
using HCMS.Services.ServiceModels.Application;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Web.ViewModels.Advert;
using HCMS.Web.ViewModels.Application;

namespace HCMS.Web.AutoMapperProfiles
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<ApplicationFormModel, ApplicationDto>().ReverseMap();
        }
    }
}
