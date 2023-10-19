using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.Employee;

namespace HCMS.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IUserRepository userRepository;


        public EmployeeService(IEmployeeRepository employeeRepository, IUserRepository userRepository)
        {
            this.employeeRepository = employeeRepository;
            this.userRepository = userRepository;
        }


        public Task<Employee> UpdateEmployeeAsync(EmployeeFormModel model)
        {
            Employee employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PhotoUrl = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                AddDate = DateTime.Now,
                CompanyId = null,
                Company = null,
                UserId = null,
                User = null,
                LocationId = null,
                Location = null
            };



            throw new NotImplementedException();
        }
    }
}
