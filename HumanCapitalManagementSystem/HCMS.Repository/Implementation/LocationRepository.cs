using AutoMapper;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository.Implementation
{
    internal class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public LocationRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<Location?> GetLocationByCountryStateAddressAsync(string country, string state, string address)
        {
            return await dbContext.Locations
                .Where(l => l.Country == country && l.State == state && l.Address == address)
                .FirstOrDefaultAsync();
        }

        public async Task<Location> GetLocationById(Guid id)
        {
            try
            {
                return await dbContext.Locations.FirstAsync(l => l.Id == id);
            }
            catch (Exception)
            {
                throw new Exception("Location not found!");
            }
        }

        public async Task UpdateLocationAsync(Location locationInfo)
        {
            try
            {
                Location location = await dbContext.Locations.FirstAsync(l => l.Id == locationInfo.Id);
                location = mapper.Map<Location>(locationInfo);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
