using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext dbContext;

        public LocationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Location?> GetLocationByCountryStateAddressAsync(string country, string state, string address)
        {
            return await dbContext.Locations
                .Where(l => l.Country == country && l.State == state && l.Address == address)
                .FirstOrDefaultAsync();
        }
    }
}
