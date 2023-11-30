using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Services.ServiceModels.Advert;

namespace HCMS.Services.AutoMapperProfiles
{
    public class AdvertMappingProfile : Profile
    {
        public AdvertMappingProfile()
        {
            CreateMap<QueryPageWrapClass<Advert>, AdvertQueryDtoResult>()
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();

            CreateMap<AdvertAddDto, Advert>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src =>src.Position.ToString()))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.ToString()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToString()))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary.ToString()));

            CreateMap<Advert, AdvertAddDto>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => new Position(src.Position)))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => new Department(src.Department)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new Description(src.Description)))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => new Salary(src.Salary)));
            
            CreateMap<Advert, AdvertViewDto>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new LocationStruct(src.Company.Location!.Address, src.Company.Location.State, src.Company.Location.Country)))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => new Name(src.Company.Name)));
        }
    }
}
