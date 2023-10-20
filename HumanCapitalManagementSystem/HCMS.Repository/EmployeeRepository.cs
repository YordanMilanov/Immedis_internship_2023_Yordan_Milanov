using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task AddEmployeeAsync(Employee employee)
        {
            try
            {
                await dbContext.Employees.AddAsync(employee);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
   
        }

        public async Task<Employee?> GetEmployeeByUserIdAsync(Guid id)
        {
           return await dbContext.Employees
               .Include(e => e.Location)
               .AsTracking()
               .FirstOrDefaultAsync(e => e.UserId == id);
        }

        public async Task<bool> ExistsEmployeeByUserIdAsync(Guid id)
        {
            return await dbContext.Employees
                .Select(e => e.Id == id)
                .AnyAsync();
        }
    }
}
