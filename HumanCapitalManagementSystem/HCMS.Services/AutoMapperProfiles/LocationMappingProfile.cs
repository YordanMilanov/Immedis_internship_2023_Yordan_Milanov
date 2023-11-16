using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Data.Models;
using HCMS.Services.ServiceModels.Location;

namespace HCMS.Services.AutoMapperProfiles;
public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile()
        {
        CreateMap<Location, LocationDto>();
        CreateMap<LocationDto, Location>();

        CreateMap<Location, LocationStruct>()
            .ConstructUsing(src => new LocationStruct(src.Address, src.State, src.Country));

        CreateMap<LocationStruct, Location>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.GetAddress()))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.GetState()))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.GetCountry()));
    }
    }
