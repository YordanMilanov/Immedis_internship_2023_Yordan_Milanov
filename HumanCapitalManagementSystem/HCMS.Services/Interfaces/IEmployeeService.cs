using HCMS.Data.Models;
using HCMS.Web.ViewModels.Employee;

namespace HCMS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> UpdateEmployeeAsync(EmployeeFormModel model);
    }
}
