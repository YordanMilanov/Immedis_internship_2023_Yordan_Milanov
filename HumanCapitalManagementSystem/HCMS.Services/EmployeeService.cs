using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IUserRepository userRepository;
        private readonly ILocationRepository locationRepository;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;


        public EmployeeService(IEmployeeRepository employeeRepository, IUserRepository userRepository, ILocationRepository locationRepository, ApplicationDbContext dbContext, IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.userRepository = userRepository;
            this.locationRepository = locationRepository;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }



        public async Task UpdateEmployeeAsync(EmployeeDto model)
        {
            //take the employee and track it from the db
            Employee? employee = await dbContext.Employees
                .Include(employee => employee.Location!)
                .FirstOrDefaultAsync(e => e.UserId == model.UserId);

            //create new employee if employee == null
            if (employee == null)
            {
                //create new employee
                Employee UpdatedEmployee = mapper.Map<Employee>(model);
                employee = UpdatedEmployee;

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
                employee.PhotoUrl = model.PhotoUrl.ToString();
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
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
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

            //EmployeeDto employeeDto2 = new EmployeeDto
            //{
            //    Id = employee.Id,
            //    FirstName = employee.FirstName,
            //    LastName = employee.LastName,
            //    Email = employee.Email,
            //    PhoneNumber = employee.PhoneNumber,
            //    PhotoUrl = employee.PhotoUrl,
            //    DateOfBirth = employee.DateOfBirth,
            //    AddDate = employee.AddDate,
            //    UserId = (Guid)employee.UserId!,
            //};

            Location location = employee.Location!;
            if (location != null)
            {
                employeeDto.Location= new LocationStruct(location.Address, location.State, location.Country);
            }

            return employeeDto;
        }
    }
}
