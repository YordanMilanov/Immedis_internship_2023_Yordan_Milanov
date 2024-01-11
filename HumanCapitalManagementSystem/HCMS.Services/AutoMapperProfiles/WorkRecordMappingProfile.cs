using AutoMapper;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Services.AutoMapperProfiles;
public class WorkRecordMappingProfile : Profile
{
    public WorkRecordMappingProfile()
    {
        CreateMap<WorkRecord, WorkRecordDto>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));


        CreateMap<WorkRecordDto, WorkRecord>()
            .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore());

        CreateMap<QueryPageWrapClass<WorkRecord>, QueryDtoResult<WorkRecordDto>>();
    }
}
