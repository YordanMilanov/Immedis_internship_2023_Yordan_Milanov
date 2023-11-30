using AutoMapper;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;
using HCMS.Services.ServiceModels.Advert;
using HCMS.Services.ServiceModels.BaseClasses;

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

        public async Task<AdvertQueryDtoResult> GetCurrentPageAsync(AdvertQueryDto advertQueryDto)
        {
            try
            {
                QueryDto queryDto = mapper.Map<QueryDto>(advertQueryDto);
                QueryParameterClass parameters = mapper.Map<QueryParameterClass>(queryDto);
                QueryPageWrapClass<Advert> result = await this.advertRepository.GetCurrentPageAsync(parameters, advertQueryDto.RemoteOption, advertQueryDto.CompanyId);

                AdvertQueryDtoResult modelToReturn = mapper.Map<AdvertQueryDtoResult>(result);
                modelToReturn.CurrentPage = queryDto.CurrentPage;
                modelToReturn.OrderPageEnum = queryDto.OrderPageEnum;
                modelToReturn.ItemsPerPage = queryDto.ItemsPerPage;
                modelToReturn.SearchString = queryDto.SearchString;
                modelToReturn.RemoteOption = advertQueryDto.RemoteOption;
                modelToReturn.CompanyId = advertQueryDto.CompanyId;
                return modelToReturn;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
