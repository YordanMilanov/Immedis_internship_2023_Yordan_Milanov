using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;

namespace HCMS.Repository
{
    public class WorkRecordRepository : IWorkRecordRepository
    {
        private readonly ApplicationDbContext dbContext;

        public WorkRecordRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddWorkRecord(WorkRecord model)
        {
            try
            {
                await dbContext.WorkRecords.AddAsync(model);
                await dbContext.SaveChangesAsync();
            } catch(Exception)
            {
                throw new Exception();
            }

        }
    }
}
