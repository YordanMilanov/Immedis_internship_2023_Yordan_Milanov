using AutoMapper;
using HCMS.Services.ServiceModels.WorkRecord;
using HCMS.Web.ViewModels.WorkRecord;

namespace HCMS.Web.AutoMapperProfiles
{
    public class WorkRecordMappingProfile : Profile
    {
        public WorkRecordMappingProfile()
        {

            CreateMap<WorkRecordViewModel, WorkRecordDto>().ReverseMap();

            CreateMap<WorkRecordQueryDto, WorkRecordQueryModel>()
           .ForMember(dest => dest.SearchString, opt => opt.MapFrom(src => src.SearchString))
           .ForMember(dest => dest.OrderPageEnum, opt => opt.MapFrom(src => src.OrderPageEnum))
           .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
           .ForMember(dest => dest.TotalWorkRecords, opt => opt.MapFrom(src => src.TotalWorkRecords))
           .ForMember(dest => dest.WorkRecordsPerPage, opt => opt.MapFrom(src => src.WorkRecordsPerPage))
           .ForMember(dest => dest.WorkRecords, opt => opt.MapFrom(src => src.WorkRecords)); //here we map from WorkRecordDto to WorkRecordViewModel
          
            CreateMap<WorkRecordQueryDto, WorkRecordQueryModel>().ReverseMap();
        }
    }
}
