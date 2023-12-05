using HCMS.Services.ServiceModels.Application;
using HCMS.Services.ServiceModels.BaseClasses;

namespace HCMS.Services.Interfaces
{
    public interface IApplicationService
    {
        Task acceptApplicationByIdAsync(Guid id);
        Task declineApplicationByIdAsync(Guid id);
        Task AddAsync(ApplicationDto applicationDto);
        Task<QueryDtoResult<ApplicationDto>> GetCurrentPageByAdvertAsync(QueryDto model, Guid advertId);
    }
}
