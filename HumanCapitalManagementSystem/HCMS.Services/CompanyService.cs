using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.Company;

namespace HCMS.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository CompanyRepository;
        private readonly IEmployeeRepository employeeRepository;

        public CompanyService(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository)
        {
            this.CompanyRepository = companyRepository;
            this.employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<string>> GetAllCompanyNames()
        {
            return await CompanyRepository.GetAllCompanyNamesAsync();
        }

        public async Task<CompanyViewModel> GetCompanyByUserId(Guid id)
        {
            Company? company = await employeeRepository.GetEmployeeCompanyByEmployeeUserIdAsync(id);

            if (company == null)
            {
                return new CompanyViewModel();
            }

            CompanyViewModel companyViewModel = new CompanyViewModel()
            {
                Name = company.Name,
                Description = company.Description,
                IndustryField = company.IndustryField,
            };
            
            if (company.Location != null)
            {
               companyViewModel.Country = company.Location.Country;
               companyViewModel.State = company.Location.State;
               companyViewModel.Address = company.Location.Address;
            }

            return companyViewModel;
        }
    }
}
