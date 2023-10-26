using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Data.Models;

namespace HCMS.Web.Api.AutoMapperProfiles;
    public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile()
        {
            CreateMap<LocationStruct, Data.Models.Location>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.GetAddress()))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.GetState()))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.GetCountry()));

            CreateMap<Data.Models.Location, LocationStruct>()
             .ConstructUsing(src => new LocationStruct(src.Address, src.State, src.Country));
    }
    }
