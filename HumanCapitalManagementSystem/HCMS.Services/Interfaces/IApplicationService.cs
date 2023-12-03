using HCMS.Services.ServiceModels.Application;

namespace HCMS.Services.Interfaces
{
    public interface IApplicationService
    {
        Task AddAsync(ApplicationDto applicationDto);
    }
}
