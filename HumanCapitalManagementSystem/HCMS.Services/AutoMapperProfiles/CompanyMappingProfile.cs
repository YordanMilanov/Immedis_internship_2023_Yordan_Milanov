using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Company;
using HCMS.Services.ServiceModels.Employee;

namespace HCMS.Web.Api.AutoMapperProfiles
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            // Mapping from CompanyDto to Company
            CreateMap<CompanyDto, Company>();

            CreateMap<Company, CompanyDto>()
          .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new LocationStruct(src.Location!.Address, src.Location!.State, src.Location!.Country)));

            CreateMap<QueryPageWrapClass<Company>, QueryDtoResult<CompanyDto>>()
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();
        }
    }
}
