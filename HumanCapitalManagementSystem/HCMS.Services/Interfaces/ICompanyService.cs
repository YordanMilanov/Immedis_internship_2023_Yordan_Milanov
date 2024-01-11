using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Company;

namespace HCMS.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<string>> GetAllCompanyNamesAsync();

        Task<CompanyDto> GetCompanyDtoByEmployeeIdAsync(Guid employeeId);

        Task<CompanyDto> GetCompanyDtoByCompanyNameAsync(string companyName);
        Task<QueryDtoResult<CompanyDto>> GetCompaniesPageAndTotalCountAsync(QueryDto model);
        Task<CompanyDto> GetCompanyDtoByIdAsync(Guid id);
        Task AddCompanyDtoAsync(CompanyDto model);
        Task EditCompanyDtoAsync(CompanyDto model);
        Task<string> GetCompanyNameById(Guid guid);
    }
}
