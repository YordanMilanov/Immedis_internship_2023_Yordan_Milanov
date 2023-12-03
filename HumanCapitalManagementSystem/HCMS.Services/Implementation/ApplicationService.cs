using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Application;

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
            catch(Exception)
            {
                throw;
            }
        }
    }
}
