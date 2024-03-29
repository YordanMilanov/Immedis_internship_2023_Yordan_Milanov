﻿using AutoMapper;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.BaseClasses;
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
                //update existing employee

                if (model.Id != Guid.Empty)
                {
                    await employeeRepository.UpdateEmployeeAsync(employee);
                }
                //create new employee if employee == null
                else
                {
                    await employeeRepository.AddEmployeeAsync(employee);
                }
            }
            catch (Exception ex)
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

        public async Task<QueryDtoResult<EmployeeDto>> GetCurrentPageAsync(QueryDto model)
        {
            try
            {
                QueryParameterClass parameters = mapper.Map<QueryParameterClass>(model);
                QueryPageWrapClass<Employee> result = await this.employeeRepository.GetCurrentPageAsync(parameters);

                QueryDtoResult<EmployeeDto> modelToReturn = mapper.Map<QueryDtoResult<EmployeeDto>>(result);
                modelToReturn.CurrentPage = model.CurrentPage;
                modelToReturn.OrderPageEnum = model.OrderPageEnum;
                modelToReturn.ItemsPerPage = model.ItemsPerPage;
                modelToReturn.SearchString = model.SearchString;
                return modelToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveEmployeeCompanyByIdAsync(Guid id)
        {
            try
            {
                await this.employeeRepository.LeaveCompanyByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetEmployeeFullNameById(Guid id)
        {
            try
            {
                return await this.employeeRepository.GetEmployeeFullNameById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsEmployeeEmailExistsAsync(EmployeeDto model)
        {
            return await this.employeeRepository.IsEmployeeEmailUsedByAnotherEmployee(model.Email.ToString(), model.Id);
        }

        public async Task<bool> IsEmployeePhoneNumberExistsAsync(EmployeeDto model)
        {
            return await this.employeeRepository.IsEmployeePhoneNumberUsedByAnotherEmployee(model.PhoneNumber.ToString(), model.Id);
        }

        public async Task<EmployeeDto> GetEmployeeDtoByIdAsync(Guid id)
        {
            return mapper.Map<EmployeeDto>(await employeeRepository.GetEmployeeByIdAsync(id));
        }
    }
}
