using AutoMapper;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Advert;

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
    }
}
