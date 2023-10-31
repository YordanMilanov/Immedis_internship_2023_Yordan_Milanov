using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.ServiceModels.WorkRecord;
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

        public async Task<List<WorkRecord>> GetWorkRecordsPageAsync(WorkRecordQueryDto searchModel)
        {
            try
            {
                IQueryable<WorkRecord> query = dbContext.WorkRecords;
                //check if the page is for certain employee or no
                  query = query.Where(wr => wr.EmployeeId == searchModel.EmployeeId);

                //check the search string
                if (searchModel.SearchString != null)
                {
                   query = query.Where(wr => wr.Position.Contains(searchModel.SearchString!) || wr.Department!.Contains(searchModel.SearchString!));

                }

                //check the order
                switch (searchModel.OrderPageEnum)
                {
                    case OrderPageEnum.Newest:
                        query = query.OrderBy(wr => wr.StartDate);
                        break;
                    case OrderPageEnum.Oldest:
                        query = query.OrderByDescending(wr => wr.StartDate);
                        break;
                    case OrderPageEnum.SalaryAscending:
                        query = query.OrderByDescending(wr => wr.Salary);
                        break;
                    case OrderPageEnum.SalaryDescending:
                        query = query.OrderByDescending(wr => wr.Salary);
                        break;
                }

                //Pagination
                int workRecordsCountToSkip = (searchModel.CurrentPage - 1) * searchModel.WorkRecordsPerPage;
                query = query.Skip(workRecordsCountToSkip).Take(searchModel.WorkRecordsPerPage);

                List<WorkRecord> records = await query.ToListAsync();
                return records;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            };
        }

        public async Task<int> WorkRecordsCountByEmployeeIdAsync(Guid employeeId)
        {
            try
            {
                return await dbContext.WorkRecords.Where(wr => wr.EmployeeId == employeeId).CountAsync();
            }
            catch(Exception)
            {
                throw new Exception("No work records was found");
            }
        }
    }
}
