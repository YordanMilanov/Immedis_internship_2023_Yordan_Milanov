using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Company;
using HCMS.Web.ViewModels.Company;

namespace HCMS.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<string>> GetAllCompanyNamesAsync()
        {
            return await companyRepository.GetAllCompanyNamesAsync();
        }


        public async Task<CompanyDto> GetCompanyDtoByEmployeeIdAsync(Guid employeeId)
        {
            try
            {
               Company company = await companyRepository.GetCompanyByEmployeeIdAsync(employeeId);
                CompanyDto companyDto = mapper.Map<CompanyDto>(company);
                return companyDto;
            } catch (Exception) { 
                throw new Exception("Unexpected error occurred!");
            }
        }
    }
}
