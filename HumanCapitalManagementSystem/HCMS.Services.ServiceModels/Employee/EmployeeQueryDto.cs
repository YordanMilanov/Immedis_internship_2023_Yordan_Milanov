using HCMS.Common;
namespace HCMS.Services.ServiceModels.Employee
{
    public class EmployeeQueryDto
    {
        public string? SearchString { get; set; }
        public OrderPageEnum OrderPageEnum { get; set; }
        public int CurrentPage { get; set; }
        public int TotalEmployees { get; set; }
        public int EmployeesPerPage { get; set; }
        public IEnumerable<EmployeeDto> Employees { get; set; }
    }
}
