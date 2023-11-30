using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Services.ServiceModels.Advert;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Web.ViewModels.Advert;

namespace HCMS.Web.AutoMapperProfiles
{
    public class AdvertMappingProfile : Profile
    {
        public AdvertMappingProfile()
        {
            CreateMap<AdvertQueryDto, QueryDto>();
            CreateMap<QueryDto, AdvertQueryDto>();
            CreateMap<AdvertPageQueryModel, QueryDto>().ReverseMap();
            CreateMap<AdvertPageQueryModel, AdvertResultQueryModel>();
            CreateMap<AdvertQueryDto, AdvertPageQueryModel>().ReverseMap();
            CreateMap<AdvertQueryDto, AdvertResultQueryModel>().ReverseMap();

            CreateMap<AdvertFormModel, AdvertAddDto>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => new Position(src.Position)))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => new Department(src.Department)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new Description(src.Description)))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => new Salary(src.Salary)));


            CreateMap<AdvertViewDto, AdvertViewModel>()
              .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()))
              .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.ToString()))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToString()))
              .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary.GetValue()))
              .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.GetCountry()))
              .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Location.GetState()))
              .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName.ToString()));

        }
    }
}
