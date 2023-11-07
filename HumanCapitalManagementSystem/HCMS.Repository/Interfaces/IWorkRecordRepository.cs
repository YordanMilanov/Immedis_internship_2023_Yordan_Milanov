using HCMS.Common;
using HCMS.Data.Models;
using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Repository.Interfaces
{
    public interface IWorkRecordRepository
    {
       public Task AddWorkRecordAsync(WorkRecord model);

        public Task<List<WorkRecord>> AllWorkRecordsByEmployeeIdAsync(Guid id);

        public Task<List<WorkRecord>> AllWorkRecordsAsync();

        // Returns total Count with this settings and the records
        public Task<(int, List<WorkRecord>)> GetWorkRecordsPageAndTotalCountAsync(string? searchString, OrderPageEnum orderPageEnum, int currentPage, int perPage, Guid employeeId);

        public Task<int> WorkRecordsCountByEmployeeIdAsync(Guid employeeId);

        public Task DeleteById(Guid id);
    }
}
