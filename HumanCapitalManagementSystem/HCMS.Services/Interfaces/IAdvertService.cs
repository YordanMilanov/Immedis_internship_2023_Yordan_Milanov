using HCMS.Services.ServiceModels.Advert;
using HCMS.Services.ServiceModels.BaseClasses;

namespace HCMS.Services.Interfaces
{
    public interface IAdvertService
    {
        Task AddAsync(AdvertAddDto model);
        Task<AdvertQueryDtoResult> GetCurrentPageByCompanyAsync(AdvertQueryDto AdvertQueryDto, Guid companyId);
    }
}
