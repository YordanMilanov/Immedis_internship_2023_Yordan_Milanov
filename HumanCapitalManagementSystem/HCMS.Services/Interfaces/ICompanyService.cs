using HCMS.Services.ServiceModels.Company;
using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<string>> GetAllCompanyNamesAsync();

        Task<CompanyDto> GetCompanyDtoByEmployeeIdAsync(Guid employeeId);

        Task<CompanyDto> GetCompanyDtoByCompanyNameAsync(string companyName);
        Task<(int, List<CompanyDto>)> GetCompaniesPageAndTotalCountAsync(CompanyQueryDto model);
        Task<CompanyDto> GetCompanyDtoByIdAsync(Guid id);
        Task AddCompanyDtoAsync(CompanyDto model);
        Task EditCompanyDtoAsync(CompanyDto model);
    }
}
