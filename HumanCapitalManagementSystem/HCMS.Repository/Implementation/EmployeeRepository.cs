using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
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

                if (await dbContext.Employees.AnyAsync(e => e.PhoneNumber == model.PhoneNumber))
                {
                    throw new Exception("Phone number is already used!");
                } 
                else if (await dbContext.Employees.AnyAsync(e => e.Email == model.Email))
                {
                    throw new Exception("Email is already used!");

                }

                await dbContext.Employees.AddAsync(employeeToSave);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Employee?> GetEmployeeByUserIdAsync(Guid id)
        {
            try
            {
                return await dbContext.Employees
                    .Include(e => e.Location)
                    .Include(e => e.Company)
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

        public async Task<bool> IsEmployeeEmailUsedByAnotherEmployee(string email, Guid Id)
        {
            return await dbContext.Employees.Where(e => e.Id != Id).AnyAsync(e => e.Email == email);
        }

        public async Task<bool> IsEmployeePhoneNumberUsedByAnotherEmployee(string phoneNumber, Guid Id)
        {
            return await dbContext.Employees.Where(e => e.Id != Id).AnyAsync(e => e.PhoneNumber == phoneNumber);
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
                return await dbContext.Employees.Include(e => e.Company).FirstAsync(e => e.Id == id);
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

                if (await dbContext.Employees.AnyAsync(e => e.PhoneNumber == model.PhoneNumber && e.Id != model.Id))
                {
                    throw new Exception("Phone number is already used by another employee!");
                }
                else if (await dbContext.Employees.AnyAsync(e => e.Email == model.Email && e.Id != model.Id))
                {
                    throw new Exception("Email is already used by another employee!");

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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<QueryPageWrapClass<Employee>> GetCurrentPageAsync(QueryParameterClass parameters)
        {
            try
            {
                IQueryable<Employee> query = dbContext.Employees.Include(e => e.Location).Include(e => e.Company);
                //check the search string
                if (parameters.SearchString != null)
                {
                    query = query.Where(e =>
                    e.FirstName.Contains(parameters.SearchString!) ||
                    e.LastName.Contains(parameters.SearchString!) ||
                    e.Email.Contains(parameters.SearchString!) ||
                    e.PhoneNumber.Contains(parameters.SearchString!) ||
                    e.Location!.Address.Contains(parameters.SearchString!) ||
                    e.Location.State.Contains(parameters.SearchString!) ||
                    e.Location.Country.Contains(parameters.SearchString!) ||
                    e.Company!.Name.Contains(parameters.SearchString!));
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
                int employeesCountToSkip = (parameters.CurrentPage - 1) * parameters.ItemsPerPage;
                query = query.Skip(employeesCountToSkip).Take(parameters.ItemsPerPage);

                List<Employee> employees = await query.ToListAsync();
                return new QueryPageWrapClass<Employee>()
                {
                    Items = employees,
                    TotalItems = countOfElements
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            };
        }

        public async Task LeaveCompanyByIdAsync(Guid id)
        {
            try
            {
                Employee employee = await this.dbContext.Employees.FirstAsync(e => e.Id == id);
                employee.CompanyId = null;
                employee.Company = null;
                await this.dbContext.SaveChangesAsync();
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetEmployeeFullNameById(Guid id)
        {
            try
            {

                Employee employee = await this.dbContext.Employees.FirstAsync(e => e.Id == id)!;

                return $"{employee.FirstName} {employee.LastName}";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            try
            {
                return await this.dbContext.Employees.FirstAsync(e => e.Email == email);
            } 
            catch(Exception)
            {
                throw new Exception("Employee with this email does not exist!");
            }
        }
    }
}
