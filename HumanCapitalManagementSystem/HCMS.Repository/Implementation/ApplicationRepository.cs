using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.Data.SqlClient;

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
    }
}
