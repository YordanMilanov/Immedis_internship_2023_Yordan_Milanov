using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Company;

namespace HCMS.Services.Implementation
{
    internal class CompanyService : ICompanyService
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
                if (company == null)
                {
                    throw new Exception("");
                }
                CompanyDto companyDto = mapper.Map<CompanyDto>(company);
                return companyDto;
            }
            catch (Exception)
            {
                throw new Exception("Employee company not found!");
            }
        }

        public async Task<CompanyDto> GetCompanyDtoByCompanyNameAsync(string companyName)
        {
            try
            {
                Company company = await companyRepository.GetCompanyByNameAsync(companyName);
                CompanyDto companyDto = mapper.Map<CompanyDto>(company);
                return companyDto;
            }
            catch
            {
                throw new Exception("No company was found!");
            }
        }
    }
}
