using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Data.Models;
using HCMS.Services.ServiceModels.Company;

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


        }
    }
}
