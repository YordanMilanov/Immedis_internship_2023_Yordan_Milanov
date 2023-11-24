using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;

namespace HCMS.Repository.Interfaces
{
    public interface IRecommendationRepository
    {
        Task AddAsync(Recommendation recommendation);
        Task<QueryPageWrapClass<Recommendation>> GetCurrentPageAsync(QueryParameterClass parameters, Guid companyId);
    }
}
