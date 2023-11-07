using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Company;
using HCMS.Services.ServiceModels.WorkRecord;

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

        public async Task<(int, List<CompanyDto>)> GetCompaniesPageAndTotalCountAsync(CompanyQueryDto model)
        {
            try
            {
                object result = await this.companyRepository.GetCurrentPageAndTotalCountAsync(model.CurrentPage, model.SearchString, model.CompaniesPerPage);

                if (result is (int totalCount, List<Company> companies))
                {
                    List<CompanyDto> companyDtos = companies.Select(c => mapper.Map<CompanyDto>(c)).ToList();
                    return (totalCount, companyDtos);
                }
                else
                {
                    throw new Exception("Unexpected error occurred!");
                };
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
