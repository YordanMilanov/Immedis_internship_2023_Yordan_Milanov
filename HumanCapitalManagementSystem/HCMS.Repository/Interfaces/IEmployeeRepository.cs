using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> UpdateEmployeeAsync (Employee model);
    }
}
