using HCMS.Services.ServiceModels.Application;
using HCMS.Services.ServiceModels.BaseClasses;

namespace HCMS.Services.Interfaces
{
    public interface IApplicationService
    {
        Task AddAsync(ApplicationDto applicationDto);
        Task<QueryDtoResult<ApplicationDto>> GetCurrentPageByAdvertAsync(QueryDto model, Guid advertId);
    }
}
