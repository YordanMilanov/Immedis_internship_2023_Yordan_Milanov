using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Location;

namespace HCMS.Services.Implementation
{
    internal class LocationService : ILocationService
    {
        private readonly ILocationRepository locationRepository;
        private readonly IMapper mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            this.locationRepository = locationRepository;
            this.mapper = mapper;
        }

        public async Task<LocationDto> GetLocationDtoByIdAsync(Guid id)
        {

            try
            {
                Location location = await locationRepository.GetLocationById(id);
                LocationDto locationDto = mapper.Map<LocationDto>(location);
                return locationDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
