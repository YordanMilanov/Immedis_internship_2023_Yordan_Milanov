using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository.Implementation
{
    internal class ApplicationRepository : IApplicationRepository
    {

        private readonly ApplicationDbContext dbContext;

        public ApplicationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Application application)
        {
            try
            {
                application.AddDate = DateTime.Now;
              await this.dbContext.AddAsync(application);
              await this.dbContext.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                //2627 - unique constraint violation code
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                {
                    string constraintName = "UC_AdvertId_FromEmployeeId";
                    if (sqlException.Message.Contains(constraintName))
                    {
                        throw new Exception("You cannot apply twice for the same job offer. The employer will reply to your application!");
                    }
                }
                else
                {
                    throw new Exception("Unexpected error occurred while trying to persist your application. Please try again later!");
                }

            }
        }

        public async Task<QueryPageWrapClass<Application>> GetCurrentByAdvertPageAsync(QueryParameterClass parameters, Guid advertId)
        {
            try
            {
                IQueryable<Application> query = dbContext.Applications
                    .Where(a => a.AdvertId == advertId)
                    .Include(a => a.Employee)
                    .Include(a => a.Advert)
                    .ThenInclude(a => a.Company);
               

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
                int applicationsCountToSkip = (parameters.CurrentPage - 1) * parameters.ItemsPerPage;
                query = query.Skip(applicationsCountToSkip).Take(parameters.ItemsPerPage);

                List<Application> applications = await query.ToListAsync();
                return new QueryPageWrapClass<Application>()
                {
                    Items = applications,
                    TotalItems = countOfElements
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            };
        }

        public async Task acceptApplicationByIdAsync(Guid id)
        {
            try
            {
                Application application = await this.dbContext.Applications.Include(a => a.Advert).FirstAsync(a => a.Id == id);
                Guid companyId = application.Advert.CompanyId;
                Guid employeeId = application.FromEmployeeId;
                Employee employee = await this.dbContext.Employees.FirstAsync(e => e.Id == employeeId);
                employee.CompanyId = companyId;
                this.dbContext.Applications.Remove(application);
                await this.dbContext.SaveChangesAsync();
            } 
            catch(Exception)
            {
                throw;
            }
        }

        public async Task declineApplicationByIdAsync(Guid id)
        {
            try
            {
                Application application = await this.dbContext.Applications.FirstAsync(a => a.Id == id);
                this.dbContext.Applications.Remove(application);
                await this.dbContext.SaveChangesAsync();
            } 
            catch(Exception)
            {
                throw;
            }
        }
    }
}
