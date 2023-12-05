using AutoMapper;
using HCMS.Services.ServiceModels.Application;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Web.ViewModels.Application;
using HCMS.Web.ViewModels.BaseViewModel;

namespace HCMS.Web.AutoMapperProfiles
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<ApplicationFormModel, ApplicationDto>().ReverseMap();
            CreateMap<ApplicationDto, ApplicationViewModel>().ReverseMap();

            CreateMap<ResultQueryModel<ApplicationViewModel>, QueryDto>();
            CreateMap<QueryDtoResult<ApplicationDto>, ResultQueryModel<ApplicationViewModel>>();
        }
    }
}
