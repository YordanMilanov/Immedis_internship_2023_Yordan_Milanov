using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
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

        public async Task<QueryPageWrapClass<Recommendation>> GetCurrentPageAsync(QueryParameterClass parameters, Guid companyId)
        {
            try
            {
                IQueryable<Recommendation> query = dbContext.Recommendations
                    .Where(r => r.Company.Id == companyId)
                    .Include(r => r.ForEmployee)
                    .Include(r => r.FromEmployee)
                    .ThenInclude(e => e.Company);
                //check the search string
                if (parameters.SearchString != null)
                {
                    query = query.Where(r =>
                    r.Description.Contains(parameters.SearchString!) ||

                    r.FromEmployee.Company!.Name.Contains(parameters.SearchString!) ||

                    r.ForEmployee.FirstName.Contains(parameters.SearchString!) ||
                    r.ForEmployee.LastName.Contains(parameters.SearchString!) ||
                    r.ForEmployee.Email.Contains(parameters.SearchString!) ||
                    r.ForEmployee.PhoneNumber.Contains(parameters.SearchString!));
                }

                int countOfElements = query.Count();

                //check the order
                switch (parameters.OrderPageEnum)
                {
                    case OrderPageEnum.Newest:
                        query = query.OrderBy(e => e.RecommendDate);
                        break;
                    case OrderPageEnum.Oldest:
                        query = query.OrderByDescending(e => e.RecommendDate);
                        break;
                }

                //Pagination
                int recommendationsCountToSkip = (parameters.CurrentPage - 1) * parameters.ItemsPerPage;
                query = query.Skip(recommendationsCountToSkip).Take(parameters.ItemsPerPage);

                List<Recommendation> recommendations = await query.ToListAsync();
                return new QueryPageWrapClass<Recommendation>()
                {
                    Items = recommendations,
                    TotalItems = countOfElements
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            };
        }
    }
}
