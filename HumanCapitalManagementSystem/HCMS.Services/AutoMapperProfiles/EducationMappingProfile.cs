using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Data.Models;
using HCMS.Services.ServiceModels.Education;

namespace HCMS.Web.Api.AutoMapperProfiles
{
    public class EducationMappingProfile : Profile
    {
        public EducationMappingProfile()
        {
            CreateMap<Education, EducationDto>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new LocationStruct(src.Location!.Address, src.Location.State, src.Location.Country)));

            CreateMap<EducationDto, Education>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Location { Address = src.Location.GetAddress(), State = src.Location.GetState(), Country = src.Location.GetCountry() }));
        }
    }
}
