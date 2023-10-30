using HCMS.Common;
using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IWorkRecordRepository
    {
       public Task AddWorkRecordAsync(WorkRecord model);

        public Task<List<WorkRecord>> AllWorkRecordsByEmployeeIdAsync(Guid id);

        public Task<List<WorkRecord>> AllWorkRecordsAsync();

        public Task<List<WorkRecord>> SearchStringAndFilteredAsync(string search, OrderPageEnum order);
    }
}
