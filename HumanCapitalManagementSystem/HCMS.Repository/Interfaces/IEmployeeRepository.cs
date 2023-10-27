﻿using HCMS.Data.Models;
using HCMS.Web.ViewModels.Company;

namespace HCMS.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync (Employee model);

        Task<Employee?> GetEmployeeByUserIdAsync (Guid id);

        Task<Company?> GetEmployeeCompanyByEmployeeUserIdAsync(Guid id);

        Task<Guid> GetEmployeeIdByUserId(Guid userId);

        Task<bool> IsEmployeePhoneNumberUsedByAnotherEmployee(string phoneNumber, Guid userId);
        Task<bool> IsEmployeeEmailUsedByAnotherEmployee(string email, Guid userId);

    }
}
