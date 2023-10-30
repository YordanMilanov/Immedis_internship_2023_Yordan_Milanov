using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository
{
    public class WorkRecordRepository : IWorkRecordRepository
    {
        private readonly ApplicationDbContext dbContext;

        public WorkRecordRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task AddWorkRecordAsync(WorkRecord model)
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

        public async Task<List<WorkRecord>> AllWorkRecordsByEmployeeIdAsync(Guid id)
        {
            try
            {
                return await dbContext.WorkRecords.Where(wr => wr.EmployeeId == id).ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       public async Task<List<WorkRecord>> AllWorkRecordsAsync()
        {
           return await dbContext.WorkRecords.ToListAsync();
        }

        public async Task<List<WorkRecord>> SearchStringAndFilteredAsync(string search, OrderPageEnum order)
        {
            //to be finished paging
            if(order == OrderPageEnum.Newest) { }

                IQueryable query = await dbContext.WorkRecords
                .Where(wr => wr.Position.Contains(search)
                    || wr.Department!.Contains(search)).AsQueryable();

            throw new NotImplementedException();
        }
    }
}
