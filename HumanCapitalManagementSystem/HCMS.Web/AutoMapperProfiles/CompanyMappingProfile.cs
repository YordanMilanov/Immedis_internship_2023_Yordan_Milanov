using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Services.ServiceModels.Company;
using HCMS.Services.ServiceModels.Education;
using HCMS.Web.ViewModels.Company;
using HCMS.Web.ViewModels.Education;

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

            CreateMap<CompanyQueryDto, CompanyQueryModel>().ReverseMap();

            CreateMap<CompanyDto, CompanyViewModel>()
    .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.GetCountry()))
    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Location.GetState()))
    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location.GetAddress()));

            CreateMap<CompanyViewModel, CompanyDto>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                    new LocationStruct(src.Address, src.State, src.Country)));
        }
    }
}
