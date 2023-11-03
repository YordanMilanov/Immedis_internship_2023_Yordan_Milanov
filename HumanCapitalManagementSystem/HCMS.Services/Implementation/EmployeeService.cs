using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Employee;

namespace HCMS.Services.Implementation
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;


        public EmployeeService(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.companyRepository = companyRepository;
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

            //arrange update object
            Employee employee = mapper.Map<Employee>(model);

            if (employee.Location!.Country != null && employee.Location.State != null)
            {
                Location location = mapper.Map<Location>(model.Location);
                location.OwnerId = employee.Id;
                location.OwnerType = employee.GetType().Name;
                employee.Location = location;
            }

            try
            {

                //create new employee if employee == null
                if (model.Id != Guid.Empty)
                {
                    await employeeRepository.UpdateEmployeeAsync(employee);
                }
                //update existing employee
                else
                {
                    await employeeRepository.AddEmployeeAsync(employee);
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        public async Task<Guid> GetEmployeeIdByUserId(Guid userId)
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
