using AutoMapper;
using HCMS.Data.Models;
using HCMS.Services.ServiceModels.Location;

namespace HCMS.Web.Api.AutoMapperProfiles;
    public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile()
        {
        CreateMap<Location, LocationDto>();
        CreateMap<LocationDto, Location>();
    }
    }
