using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository.Implementation
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RecommendationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Recommendation recommendation)
        {
            try
            {
                await this.dbContext.AddAsync(recommendation);
                await this.dbContext.SaveChangesAsync();
            }catch(DbUpdateException ex)
            {
                throw ex;
            }
        }
    }
}
