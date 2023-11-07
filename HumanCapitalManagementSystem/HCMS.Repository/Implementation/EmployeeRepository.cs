using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository.Implementation
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task AddEmployeeAsync(Employee model)
        {
            try
            {
                Employee employeeToSave = model;
                employeeToSave.Id = Guid.NewGuid();
                if(model.Location != null)
                {
                    employeeToSave.Location!.Id = Guid.NewGuid();
                    employeeToSave.LocationId = employeeToSave.Location!.Id;
                    employeeToSave.Location.OwnerId = employeeToSave.Id;
                }

                await dbContext.Employees.AddAsync(employeeToSave);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Unexpected error ocurred while trying to add your employee information!");
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
                    .ThenInclude(c => c!.Location)
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

        public async Task<Guid> GetEmployeeIdByUserIdAsync(Guid userId)
        {
            return await dbContext.Employees
                    .Where(e => e.UserId == userId)
                    .Select(e => e.Id)
                    .FirstOrDefaultAsync(); 
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            try
            {
                return await dbContext.Employees.FirstAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw new Exception("Employee not found!");
            }
        }

        public async Task UpdateEmployeeAsync(Employee model)
        {
            try
            {
                Employee employee = await dbContext.Employees.Include(e => e.Location).FirstAsync(e => e.Id == model.Id);
                if (employee == null)
                {
                    throw new Exception("Employee not found");
                }

                if (employee.LocationId == null && model.Location != null)
                {
                    Location location = model.Location!;
                    location.Id = Guid.NewGuid();
                    employee.Location = location;
                    employee.LocationId = location.Id;
                } 
                else if(employee.LocationId != null && model.Location != null)
                {
                    employee.Location!.Address = model.Location.Address;
                    employee.Location.State = model.Location.State;
                    employee.Location.Country = model.Location.Country;
                }

                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Email = model.Email;
                employee.PhoneNumber = model.PhoneNumber;
                employee.PhotoUrl = model.PhotoUrl;
                employee.DateOfBirth = model.DateOfBirth;

                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("The update operation has ben corrupted!");
            }
        }

        public async Task<(int, IEnumerable<Employee>)> GetCurrentPageAsync(int currentPage, int employeesPerPage, string? searchString, OrderPageEnum orderPageEnum)
        {
            try
            {
                IQueryable<Employee> query = dbContext.Employees.Include(e => e.Location).Include(e => e.Company);

                //check the search string
                if (searchString != null)
                {
                    query = query.Where(e =>
                    e.FirstName.Contains(searchString!) ||
                    e.LastName.Contains(searchString!) ||
                    e.Email.Contains(searchString!) ||
                    e.PhoneNumber.Contains(searchString!) ||
                    e.Location!.Address.Contains(searchString!) ||
                    e.Location.State.Contains(searchString!) ||
                    e.Location.Country.Contains(searchString!) ||
                    e.Company!.Name.Contains(searchString!));
                }

                int countOfElements = query.Count();

                //check the order
                switch (orderPageEnum)
                {
                    case OrderPageEnum.Newest:
                        query = query.OrderBy(e => e.AddDate);
                        break;
                    case OrderPageEnum.Oldest:
                        query = query.OrderByDescending(e => e.AddDate);
                        break;
                }

                //Pagination
                int employeesCountToSkip = (currentPage - 1) * employeesPerPage;
                query = query.Skip(employeesCountToSkip).Take(employeesPerPage);

                List<Employee> employees = await query.ToListAsync();
                return (countOfElements, employees);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            };
        }
    }
}
