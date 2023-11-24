using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;

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
        Task<QueryPageWrapClass<Employee>> GetCurrentPageAsync(QueryParameterClass parameters);
        Task LeaveCompanyByIdAsync(Guid id);
        Task<string> GetEmployeeFullNameById(Guid id);
        Task<Employee> GetEmployeeByEmailAsync(string email);
    }
}
