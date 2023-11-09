using AutoMapper;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.BaseClasses;
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

        public async Task<QueryDtoResult<CompanyDto>> GetCompaniesPageAndTotalCountAsync(QueryDto model)
        {
            try
            {
                QueryParameterClass parameters = mapper.Map<QueryParameterClass>(model);
                QueryPageWrapClass<Company> result = await this.companyRepository.GetCurrentPageAndTotalCountAsync(parameters);
                QueryDtoResult<CompanyDto> modelToReturn = mapper.Map<QueryDtoResult<CompanyDto>>(result);
                modelToReturn.SearchString = model.SearchString;
                modelToReturn.OrderPageEnum = model.OrderPageEnum;
                modelToReturn.ItemsPerPage = model.ItemsPerPage;
                modelToReturn.CurrentPage = model.CurrentPage;

                return modelToReturn;
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<CompanyDto> GetCompanyDtoByIdAsync(Guid id)
        {
            try
            {
                Company company = await companyRepository.GetByIdAsync(id);
                CompanyDto companyDto = mapper.Map<CompanyDto>(company);
                return companyDto;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task AddCompanyDtoAsync(CompanyDto model)
        {
            try
            {
               if(await this.companyRepository.CompanyExistsByNameAsync(model.Name))
                {
                    throw new Exception("Company name is already used please use another name!");
                }
                Company company = mapper.Map<Company>(model);
                await this.companyRepository.AddCompanyAsync(company);
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EditCompanyDtoAsync(CompanyDto model)
        {

            try
            {
                Company company = mapper.Map<Company>(model);
                await this.companyRepository.EditCompanyAsync(company);
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
