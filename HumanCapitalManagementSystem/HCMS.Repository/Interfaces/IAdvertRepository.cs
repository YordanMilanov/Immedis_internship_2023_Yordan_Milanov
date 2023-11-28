using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;

namespace HCMS.Repository.Interfaces
{
    public interface IAdvertRepository
    {
        Task AddAsync(Advert advert);
        Task<QueryPageWrapClass<Advert>> GetCurrentPageByCompanyAsync(QueryParameterClass parameters, Guid companyId);
    }
}
