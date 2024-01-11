using AutoMapper;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Application;
using HCMS.Services.ServiceModels.BaseClasses;

namespace HCMS.Services.Implementation
{
    internal class ApplicationService : IApplicationService
    {
        private readonly IMapper mapper;
        private readonly IApplicationRepository applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository, IMapper mapper)
        {
            this.applicationRepository = applicationRepository;
            this.mapper = mapper;
        }

        public async Task AddAsync(ApplicationDto applicationDto)
        {
            try
            {
                Application application = mapper.Map<Application>(applicationDto);
                await this.applicationRepository.AddAsync(application);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<QueryDtoResult<ApplicationDto>> GetCurrentPageByAdvertAsync(QueryDto model, Guid advertId)
        {
            try
            {
                QueryParameterClass parameters = mapper.Map<QueryParameterClass>(model);
                QueryPageWrapClass<Application> result = await this.applicationRepository.GetCurrentByAdvertPageAsync(parameters, advertId);

                QueryDtoResult<ApplicationDto> modelToReturn = mapper.Map<QueryDtoResult<ApplicationDto>>(result);
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
            throw new NotImplementedException();
        }

        public async Task acceptApplicationByIdAsync(Guid id)
        {
            try
            {
                await this.applicationRepository.acceptApplicationByIdAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task declineApplicationByIdAsync(Guid id)
        {
            try
            {
                await this.applicationRepository.declineApplicationByIdAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
