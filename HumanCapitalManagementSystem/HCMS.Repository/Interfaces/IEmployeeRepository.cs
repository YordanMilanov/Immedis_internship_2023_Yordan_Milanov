using HCMS.Data.Models;
namespace HCMS.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync (Employee model);

        Task<Employee?> GetEmployeeByUserIdAsync (Guid id);

        Task<bool> ExistsEmployeeByUserIdAsync(Guid id);
    }
}
