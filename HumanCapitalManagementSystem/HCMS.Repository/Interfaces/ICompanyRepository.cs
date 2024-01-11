using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;

namespace HCMS.Repository.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company> GetByIdAsync(Guid id);

        Task<Company?> GetByNameAsync(string name);

        Task<IEnumerable<Company>> GetAllCompaniesAsync();

        Task<IEnumerable<string>> GetAllCompanyNamesAsync();

        Task<Company> GetCompanyByNameAsync(string name);

        Task<Company> GetCompanyByEmployeeIdAsync(Guid employeeId);
        Task<QueryPageWrapClass<Company>> GetCurrentPageAndTotalCountAsync(QueryParameterClass parameters);

        Task<bool> CompanyExistsByNameAsync(string name);
        Task AddCompanyAsync(Company model);
        Task EditCompanyAsync(Company company);
        Task<string> GetCompanyNameById(Guid id);
    }
}
