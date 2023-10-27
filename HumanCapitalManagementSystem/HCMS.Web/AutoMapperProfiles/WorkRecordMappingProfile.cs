using AutoMapper;
using HCMS.Services.ServiceModels.WorkRecord;
using HCMS.Web.ViewModels.WorkRecord;

namespace HCMS.Web.AutoMapperProfiles
{
    public class WorkRecordMappingProfile : Profile
    {
        public WorkRecordMappingProfile()
        {
            CreateMap<WorkRecordFormModel, WorkRecordDto>()
         .ForMember(dest => dest.Id, opt => opt.Ignore()); // You may need to ignore or set the Id as appropriate
         //.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName));
            CreateMap<WorkRecordDto, WorkRecordFormModel>();
        }
    }
}
