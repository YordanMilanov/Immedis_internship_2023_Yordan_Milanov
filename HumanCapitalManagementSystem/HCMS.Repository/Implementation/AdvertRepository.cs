using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<QueryPageWrapClass<Advert>> GetCurrentPageByCompanyAsync(AdvertQueryParameterClass parameters, Guid companyId)
        {
            try
            {
                //check the remoteOption
                IQueryable<Advert> query = dbContext.Adverts
                    .Where(a => a.RemoteOption == parameters.RemoteOption)
                    .Include(a => a.Company)
                    .ThenInclude(c => c.Location);
               
                //check the search string
                if (parameters.SearchString != null)
                {
                    query = query.Where(a =>
                    a.Position.Contains(parameters.SearchString!) ||
                    a.Department.Contains(parameters.SearchString!) ||
                    a.Description.Contains(parameters.SearchString!) ||
                    a.Company.Name.Contains(parameters.SearchString!) ||
                    a.Company.Location!.Country.Contains(parameters.SearchString!) ||
                    a.Company.Location!.State.Contains(parameters.SearchString!));
                }

                int countOfElements = query.Count();

                //check the order
                switch (parameters.OrderPageEnum)
                {
                    case OrderPageEnum.Newest:
                        query = query.OrderBy(e => e.AddDate);
                        break;
                    case OrderPageEnum.Oldest:
                        query = query.OrderByDescending(e => e.AddDate);
                        break;
                }

                //Pagination
                int advertCountToSkip = (parameters.CurrentPage - 1) * parameters.ItemsPerPage;
                query = query.Skip(advertCountToSkip).Take(parameters.ItemsPerPage);

                List<Advert> adverts = await query.ToListAsync();
                return new QueryPageWrapClass<Advert>()
                {
                    Items = adverts,
                    TotalItems = countOfElements
                };
            }
            catch (Exception)
            {
                throw;
            };
        }
    }
}
