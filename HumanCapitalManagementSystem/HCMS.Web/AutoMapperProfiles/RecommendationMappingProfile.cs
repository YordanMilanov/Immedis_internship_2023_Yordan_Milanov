using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Services.ServiceModels.Recommendation;
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
        }
    }
}
