using HCMS.Services.ServiceModels.Employee;

namespace HCMS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task UpdateEmployeeAsync(EmployeeDto model);

        Task<EmployeeDto?> GetEmployeeDtoByUserIdAsync(Guid id);

        Task<Guid> GetEmployeeIdByUserId(Guid userId);

        Task UpdateEmployeeCompanyByCompanyName(Guid employeeId, string companyName);

    }
}
