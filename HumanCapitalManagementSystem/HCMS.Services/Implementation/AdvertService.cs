using AutoMapper;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Advert;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.Employee;

namespace HCMS.Services.Implementation
{
    internal class AdvertService : IAdvertService
    {
        private readonly IMapper mapper;
        private readonly IAdvertRepository advertRepository;

        public AdvertService(IAdvertRepository advertRepository, IMapper mapper)
        {
            this.advertRepository = advertRepository;
            this.mapper = mapper;
        }

        public async Task AddAsync(AdvertAddDto model)
        {
            try
            {
                Advert advert = mapper.Map<Advert>(model);
                await advertRepository.AddAsync(advert);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            throw new NotImplementedException();
        }

        public async Task<QueryDtoResult<AdvertViewDto>> GetCurrentPageByCompanyAsync(QueryDto queryDto, Guid companyId)
        {
            try
            {
                QueryParameterClass parameters = mapper.Map<QueryParameterClass>(queryDto);
                QueryPageWrapClass<Employee> result = await this.employeeRepository.GetCurrentPageAsync(parameters,);

                QueryDtoResult<EmployeeDto> modelToReturn = mapper.Map<QueryDtoResult<EmployeeDto>>(result);
                modelToReturn.CurrentPage = model.CurrentPage;
                modelToReturn.OrderPageEnum = model.OrderPageEnum;
                modelToReturn.ItemsPerPage = model.ItemsPerPage;
                modelToReturn.SearchString = model.SearchString;
                return modelToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
