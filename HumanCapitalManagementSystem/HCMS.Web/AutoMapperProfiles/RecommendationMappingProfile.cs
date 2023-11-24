using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Employee;
using HCMS.Services.ServiceModels.Recommendation;
using HCMS.Web.ViewModels.BaseViewModel;
using HCMS.Web.ViewModels.Employee;
using HCMS.Web.ViewModels.Recommendation;

namespace HCMS.Web.AutoMapperProfiles
{
    public class RecommendationMappingProfile : Profile
    {
        public RecommendationMappingProfile()
        {
            CreateMap<RecommendationFormModel, RecommendationDto>()
                .ForMember(dest => dest.RecommenderId, opt => opt.MapFrom(src => src.RecommenderId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new Description(src.Description)))
                .ForMember(dest => dest.EmployeeEmail, opt => opt.MapFrom(src => new Email(src.EmployeeEmail)))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => new Name(src.CompanyName)));

            CreateMap<RecommendationDto, RecommendationViewModel>()
               .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.RecommendedEmployee!.FirstName))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.RecommendedEmployee!.LastName))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.RecommendedEmployee!.Email))
               .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.RecommendedEmployee!.PhoneNumber))
               .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.RecommendedEmployee!.PhotoUrl))
               .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.RecommendDate, opt => opt.MapFrom(src => src.RecommendDate));

            CreateMap<ResultQueryModel<RecommendationViewModel>, QueryDto>();
            CreateMap<QueryDtoResult<RecommendationDto>, ResultQueryModel<RecommendationViewModel>>();
        }
    }
}
