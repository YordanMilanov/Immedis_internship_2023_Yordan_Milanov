using AutoMapper;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Recommendation;

namespace HCMS.Services.AutoMapperProfiles
{
    public class RecommendationMappingProfile : Profile
    {
        public RecommendationMappingProfile()
        {
            CreateMap<RecommendationDto, Recommendation>()
          .ForMember(dest => dest.FromEmployeeId, opt => opt.MapFrom(src => src.RecommenderId))
          .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToString()));

            CreateMap<Recommendation, RecommendationDto>()
         .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.FromEmployee.Company!.Name))
         .ForMember(dest => dest.EmployeeEmail, opt => opt.MapFrom(src => src.ForEmployee.Email))
         .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToString()))
         .ForMember(dest => dest.RecommendedEmployee, opt => opt.MapFrom(src => src.ForEmployee))
         .ForMember(dest => dest.RecommenderId, opt => opt.MapFrom(src => src.FromEmployeeId))
         .ReverseMap();

            CreateMap<QueryPageWrapClass<Recommendation>, QueryDtoResult<RecommendationDto>>()
        .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
        .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
        .ReverseMap();
        }
    }
}
