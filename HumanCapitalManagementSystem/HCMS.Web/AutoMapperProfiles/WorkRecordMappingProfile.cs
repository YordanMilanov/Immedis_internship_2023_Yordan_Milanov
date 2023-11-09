using AutoMapper;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.WorkRecord;
using HCMS.Web.ViewModels.BaseViewModel;
using HCMS.Web.ViewModels.WorkRecord;

namespace HCMS.Web.AutoMapperProfiles
{
    public class WorkRecordMappingProfile : Profile
    {
        public WorkRecordMappingProfile()
        {

            CreateMap<WorkRecordViewModel, WorkRecordDto>().ReverseMap();

            CreateMap<WorkRecordDto, WorkRecordFormModel>()
           .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
           .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
           .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
           .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
           .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
           .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
           .ForMember(dest => dest.AddDate, opt => opt.MapFrom(src => src.AddDate))
           .ReverseMap();

            CreateMap<QueryDtoResult<WorkRecordDto>, ResultQueryModel<WorkRecordViewModel>>();
        }
    }
}
