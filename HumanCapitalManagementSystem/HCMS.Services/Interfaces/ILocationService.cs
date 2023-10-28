using HCMS.Services.ServiceModels.Location;

namespace HCMS.Services.Interfaces
{
    public interface ILocationService
    {
        Task<LocationDto> GetLocationDtoByIdAsync(Guid id);
    }
}
