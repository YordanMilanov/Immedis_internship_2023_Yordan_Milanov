using HCMS.Services.ServiceModels;
using HCMS.Services.ServiceModels.User;
using HCMS.Web.ViewModels.Employee;

namespace HCMS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task UpdateEmployeeAsync(EmployeeDto model);

        Task<EmployeeDto?> GetEmployeeDtoByUserIdAsync(Guid id);

        Task<Guid> GetEmployeeIdByUserId(Guid userId);
    }
}
