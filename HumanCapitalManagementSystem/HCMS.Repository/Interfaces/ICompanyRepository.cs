﻿using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync(Guid id);

        Task<Company?> GetByNameAsync(string name);

        Task<IEnumerable<Company>> GetAllCompaniesAsync();

        Task<IEnumerable<string>> GetAllCompanyNamesAsync();

        Task<Company> GetCompanyByNameAsync(string name);

        Task<Company> GetCompanyByEmployeeIdAsync(Guid employeeId);
        Task <(int, IEnumerable<Company>)> GetCurrentPageAndTotalCountAsync(int currentPage, string? searchString, int companiesPerPage);
    }
}
