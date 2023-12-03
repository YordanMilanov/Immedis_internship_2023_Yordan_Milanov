using AutoMapper;
using HCMS.Data.Models;
using HCMS.Services.ServiceModels.Application;

namespace HCMS.Services.AutoMapperProfiles
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<ApplicationDto, Application>().ReverseMap();
        }
    }
}
