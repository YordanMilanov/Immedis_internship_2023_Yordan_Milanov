﻿using HCMS.Common;
using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync (Employee employee);

        Task UpdateEmployeeAsync(Employee employee);

        Task<Employee?> GetEmployeeByUserIdAsync (Guid id);

        Task<Employee> GetEmployeeByIdAsync(Guid id);

        Task<Company?> GetEmployeeCompanyByEmployeeUserIdAsync(Guid id);

        Task<Guid> GetEmployeeIdByUserIdAsync(Guid userId);

        Task<bool> IsEmployeePhoneNumberUsedByAnotherEmployee(string phoneNumber, Guid userId);
        Task<bool> IsEmployeeEmailUsedByAnotherEmployee(string email, Guid userId);
        Task<(int, IEnumerable<Employee>)> GetCurrentPageAsync(int currentPage, int employeesPerPage, string? searchString, OrderPageEnum orderPageEnum);
    }
}
