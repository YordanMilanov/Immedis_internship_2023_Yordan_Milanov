using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IAdvertRepository
    {
        Task AddAsync(Advert advert);
    }
}
