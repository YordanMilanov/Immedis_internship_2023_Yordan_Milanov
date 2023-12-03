using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IApplicationRepository
    {
        Task AddAsync(Application application);
    }
}
