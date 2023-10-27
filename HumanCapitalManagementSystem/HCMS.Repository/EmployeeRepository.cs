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
            try
            {
                return await dbContext.Employees
                    .Include(e => e.Location)
                    .AsTracking()
                    .FirstOrDefaultAsync(e => e.UserId == id);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<Company?> GetEmployeeCompanyByEmployeeUserIdAsync(Guid id)
        {
            try
            {
                return await dbContext.Employees
                    .Where(e => e.UserId == id)
                    .AsNoTracking()
                    .Include(e => e.Company)
                    .ThenInclude(c => c.Location)
                    .Select(e => e.Company)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<bool> IsEmployeeEmailUsedByAnotherEmployee(string email, Guid userId)
        {
            return await dbContext.Employees.Where(e => e.UserId != userId).AnyAsync(e => e.Email == email);
        }

        public async Task<bool> IsEmployeePhoneNumberUsedByAnotherEmployee(string phoneNumber, Guid userId)
        {
            return await dbContext.Employees.Where(e => e.UserId != userId).AnyAsync(e => e.PhoneNumber == phoneNumber);
        }

        public async Task<Guid> GetEmployeeIdByUserId(Guid userId)
        {
            try
            {
                Guid EmployeeId = await dbContext.Employees
                    .Where(e => e.UserId == userId)
                    .Select(e => e.Id)
                    .FirstAsync();
                return EmployeeId;
            } catch (Exception)
            {
                throw new Exception("Employee with your user Id not found!");
            }
        }
    }
}
