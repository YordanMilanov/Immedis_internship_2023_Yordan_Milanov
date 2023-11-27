using HCMS.Services.ServiceModels.Advert;

namespace HCMS.Services.Interfaces
{
    public interface IAdvertService
    {
        Task AddAsync(AdvertAddDto model);
    }
}
