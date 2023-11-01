using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Employee;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Services.Implementation
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;


        public EmployeeService(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, ApplicationDbContext dbContext, IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.companyRepository = companyRepository;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }



        public async Task UpdateEmployeeAsync(EmployeeDto model)
        {
            if (await employeeRepository.IsEmployeeEmailUsedByAnotherEmployee(model.Email.ToString(), new Guid(model.UserId.ToString()!)))
            {
                throw new Exception("The email you have provided is already used!");
            }
            else if (await employeeRepository.IsEmployeePhoneNumberUsedByAnotherEmployee(model.Email.ToString(), new Guid(model.UserId.ToString()!)))
            {
                throw new Exception("The phone number you have provided is already used!");
            }

            //take the employee and track it from the db
            Employee? employee = await dbContext.Employees
                .Include(employee => employee.Location!)
                .FirstOrDefaultAsync(e => e.UserId == model.UserId);

            //create new employee if employee == null
            if (employee == null)
            {
                //create new employee
                Employee createEmployee = mapper.Map<Employee>(model);
                employee = createEmployee;

                await employeeRepository.AddEmployeeAsync(employee);
            }
            //update existing employee
            else
            {
                //update user
                employee.FirstName = model.FirstName.ToString();
                employee.LastName = model.LastName.ToString();
                employee.Email = model.Email.ToString();
                employee.PhoneNumber = model.PhoneNumber.ToString();
                employee.PhotoUrl = model.PhotoUrl!.ToString();
                employee.DateOfBirth = model.DateOfBirth;

                //if existing employee does not have location -> create
                if (employee.LocationId == null)
                {

                    Location location = mapper.Map<Location>(model.Location);
                    employee.Location = location;
                    employee.Location.Id = Guid.NewGuid();
                    employee.LocationId = location.Id;
                }
                //if existing employee has location -> update
                else
                {
                    employee.Location!.Address = model.Location.GetAddress();
                    employee.Location.State = model.Location.GetState();
                    employee.Location.Country = model.Location.GetCountry();
                }

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw new Exception("Unexpected error occurred!");
                }
            }
        }

        public async Task<EmployeeDto?> GetEmployeeDtoByUserIdAsync(Guid id)
        {
            Employee? employee = await employeeRepository.GetEmployeeByUserIdAsync(id);

            if (employee == null)
            {
                return null;
            }

            EmployeeDto employeeDto = mapper.Map<EmployeeDto>(employee);
            employeeDto.UserId = id;

            Location location = employee.Location!;
            if (location != null)
            {
                employeeDto.Location = new LocationStruct(location.Address, location.State, location.Country);
            }

            return employeeDto;
        }

        public async Task<Guid?> GetEmployeeIdByUserId(Guid userId)
        {
                return await employeeRepository.GetEmployeeIdByUserIdAsync(userId);
        }

        public async Task UpdateEmployeeCompanyByCompanyName(Guid employeeId, string companyName)
        {
            try
            {
                Company company = await companyRepository.GetCompanyByNameAsync(companyName);
                Employee employee = await employeeRepository.GetEmployeeByIdAsync(employeeId);
                employee.Company = company;
                employee.CompanyId = company.Id;


                await employeeRepository.UpdateEmployeeAsync(employee);
            }
            catch
            {
                throw new Exception("Unexpected error occurred!");
            }


        }
    }
}
