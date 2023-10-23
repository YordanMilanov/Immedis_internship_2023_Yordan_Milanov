using HCMS.Data.Models;
using HCMS.Web.ViewModels.Company;

namespace HCMS.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync (Employee model);

        Task<Employee?> GetEmployeeByUserIdAsync (Guid id);

        Task<bool> ExistsEmployeeByUserIdAsync(Guid id);

        Task<Company?> GetEmployeeCompanyByEmployeeUserIdAsync(Guid id);
    }
}
