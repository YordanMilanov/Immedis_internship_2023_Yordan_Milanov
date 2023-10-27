using HCMS.Services.ServiceModels.Company;

namespace HCMS.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<string>> GetAllCompanyNamesAsync();

        Task<CompanyDto> GetCompanyDtoByEmployeeIdAsync(Guid employeeId);
    }
}
