using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;

namespace HCMS.Repository.Implementation
{
    public class AdvertRepository : IAdvertRepository
    {

        private readonly ApplicationDbContext dbContext;

        public AdvertRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Advert advert)
        {
          try
            {
                await this.dbContext.Adverts.AddAsync(advert);
                await this.dbContext.SaveChangesAsync();
            } 
            catch (Exception)
            {
                throw;
            }
        }
    }
}
