using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface ILocationRepository
    {
        Task<Location?> GetLocationByCountryStateAddressAsync(string country, string state, string address);

        Task<Location> GetLocationById(Guid id);
    }
}
