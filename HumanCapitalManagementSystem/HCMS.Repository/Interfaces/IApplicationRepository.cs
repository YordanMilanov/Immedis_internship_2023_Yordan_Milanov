using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;

namespace HCMS.Repository.Interfaces
{
    public interface IApplicationRepository
    {
        Task AddAsync(Application application);
        Task<QueryPageWrapClass<Application>> GetCurrentByAdvertPageAsync(QueryParameterClass parameters, Guid advertId);
    }
}
