using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Common.Structures;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels;
using HCMS.Web.ViewModels.Employee;
using Microsoft.EntityFrameworkCore;
using Location = HCMS.Common.Structures.Location;

namespace HCMS.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IUserRepository userRepository;
        private readonly ILocationRepository locationRepository;
        private readonly ApplicationDbContext dbContext;


        public EmployeeService(IEmployeeRepository employeeRepository, IUserRepository userRepository, ILocationRepository locationRepository, ApplicationDbContext dbContext)
        {
            this.employeeRepository = employeeRepository;
            this.userRepository = userRepository;
            this.locationRepository = locationRepository;
            this.dbContext = dbContext;
        }



        public async Task UpdateEmployeeAsync(EmployeeFormModel model)
        {
            //take the employee and track it from the db
            Employee? employee = await dbContext.Employees
                .Include(employee => employee.Location!)
                .FirstOrDefaultAsync(e => e.UserId == model.UserId);

            //create new employee if employee == null
            if (employee == null)
            {
                Data.Models.Location location = new Data.Models.Location()
                {
                    Address = model.Address,
                    State = model.State,
                    Country = model.Country
                };

                employee = new Employee
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PhotoUrl = model.PhotoUrl,
                    DateOfBirth = model.DateOfBirth,
                    AddDate = DateTime.Now,
                    UserId = model.UserId,
                    LocationId = location.Id,
                    Location = location,
                };
                await employeeRepository.AddEmployeeAsync(employee);
            }
            //update existing employee
            else
            {
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Email = model.Email;
                employee.PhoneNumber = model.PhoneNumber;
                employee.PhotoUrl = model.PhotoUrl;
                employee.DateOfBirth = model.DateOfBirth;
                //if existing employee does not have location
                if (employee.LocationId == null)
                {
                    Data.Models.Location location = new Data.Models.Location()
                    {
                        Address = model.Address,
                        State = model.State,
                        Country = model.Country
                    };
                    employee.LocationId = location.Id;
                    employee.Location = location;
                }
                //if existing employee has location
                else
                {
                    employee.Location!.Address = model.Address;
                    employee.Location.State = model.State;
                    employee.Location.Country = model.Country;
                }

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception();
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
           
            EmployeeDto employeeDto = new EmployeeDto
            {
                FirstName = new Name(employee.FirstName),
                LastName = new Name(employee.LastName),
                Email = new Email(employee.Email),
                PhoneNumber = new Phone(employee.PhoneNumber),
                PhotoUrl = new Photo(employee.PhotoUrl),
                DateOfBirth = employee.DateOfBirth,
                AddDate = employee.AddDate,
                UserId = (Guid)employee.UserId!,
            };

            Data.Models.Location location = employee.Location!;
            if (location != null)
            {
                employeeDto.Location = new Location(location.Address, location.State, location.Country);
            }

            return employeeDto;
        }
    }
}
