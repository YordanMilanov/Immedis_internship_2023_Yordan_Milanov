using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IEducationRepository
    {
        Task AddEducationAsync(Education education);

        Task UpdateEducationAsync(Education education);

        Task<Education> GetEducationByIdAsync(Guid id);

        Task<IEnumerable<Education>> GetAllEducationsByEmployeeIdAsync(Guid employeeId);
    }
}
