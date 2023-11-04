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

        public Task<List<WorkRecord>> GetWorkRecordsPageAsync(WorkRecordQueryDto searchModel);

        public Task<int> WorkRecordsCountByEmployeeIdAsync(Guid employeeId);

        public Task DeleteById(Guid id);
    }
}
