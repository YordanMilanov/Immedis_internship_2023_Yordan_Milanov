using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.ServiceModels.WorkRecord;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository.Implementation
{
    internal class WorkRecordRepository : IWorkRecordRepository
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
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        public async Task<List<WorkRecord>> AllWorkRecordsByEmployeeIdAsync(Guid id)
        {
            try
            {
                return await dbContext.WorkRecords.Where(wr => wr.EmployeeId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<WorkRecord>> AllWorkRecordsAsync()
        {
            return await dbContext.WorkRecords.ToListAsync();
        }

        public async Task<(int, List<WorkRecord>)> GetWorkRecordsPageAndTotalCountAsync(string? searchString, OrderPageEnum orderPageEnum, int currentPage, int perPage, Guid employeeId)
        {
            try
            {
                IQueryable<WorkRecord> query = dbContext.WorkRecords.Include(wr => wr.Company);
                
                //check if the page is for certain employee or no
                query = query.Where(wr => wr.EmployeeId == employeeId);

                //check the search string
                if (searchString != null)
                {
                    query = query.Where(wr => 
                    wr.Position.Contains(searchString!) || 
                    wr.Department!.Contains(searchString!) || 
                    wr.Company.Name.Contains(searchString!));
                }

                int countOfElements = query.Count();

                //check the order
                switch (orderPageEnum)
                {
                    case OrderPageEnum.Newest:
                        query = query.OrderBy(wr => wr.StartDate);
                        break;
                    case OrderPageEnum.Oldest:
                        query = query.OrderByDescending(wr => wr.StartDate);
                        break;
                    case OrderPageEnum.SalaryAscending:
                        query = query.OrderBy(wr => wr.Salary);
                        break;
                    case OrderPageEnum.SalaryDescending:
                        query = query.OrderByDescending(wr => wr.Salary);
                        break;
                }

                //Pagination
                int workRecordsCountToSkip = (currentPage - 1) * perPage;
                query = query.Skip(workRecordsCountToSkip).Take(perPage);

                List<WorkRecord> records = await query.ToListAsync();
                return (countOfElements, records);
            }
            catch (Exception ex)
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
            catch (Exception)
            {
                throw new Exception("No work records was found");
            }
        }

        public async Task DeleteById(Guid id)
        {
            try
            {
                WorkRecord workRecord = await this.dbContext.WorkRecords.FirstAsync(wr => wr.Id == id);
                this.dbContext.WorkRecords.Remove(workRecord);
                await this.dbContext.SaveChangesAsync();
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
