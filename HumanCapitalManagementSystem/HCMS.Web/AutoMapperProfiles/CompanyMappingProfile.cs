using AutoMapper;
using HCMS.Services.ServiceModels.Company;
using HCMS.Web.ViewModels.Company;

namespace HCMS.Web.AutoMapperProfiles
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            CreateMap<CompanyDto, CompanyViewModel>()
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore());

            CreateMap<CompanyViewModel, CompanyDto>()
                .ForMember(dest => dest.LocationId, opt => opt.Ignore());
        }
    }
}
