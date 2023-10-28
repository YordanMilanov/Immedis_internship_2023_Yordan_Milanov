using AutoMapper;
using HCMS.Data.Models;
using HCMS.Services.ServiceModels.Company;

namespace HCMS.Web.Api.AutoMapperProfiles
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            // Mapping from CompanyDto to Company
            CreateMap<CompanyDto, Company>()
                .ForMember(dest => dest.Location, opt => opt.Ignore()); // Ignore Location mapping

            // Mapping from Company to CompanyDto
            CreateMap<Company, CompanyDto>();
        }
    }
}
