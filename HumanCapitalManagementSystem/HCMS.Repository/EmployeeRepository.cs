using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Web.ViewModels.Employee;

namespace HCMS.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {


        private readonly ApplicationDbContext dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<Employee> UpdateEmployeeAsync(Employee employee)
        {

            return null;
        }
    }
}
