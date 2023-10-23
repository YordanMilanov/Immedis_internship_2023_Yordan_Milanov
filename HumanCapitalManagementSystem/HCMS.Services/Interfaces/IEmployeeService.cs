
using HCMS.Services.ServiceModels;
using HCMS.Web.ViewModels.Employee;

namespace HCMS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task UpdateEmployeeAsync(EmployeeFormModel model);

        Task<EmployeeDto?> GetEmployeeDtoByUserIdAsync(Guid id);
    }
}
