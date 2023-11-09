using HCMS.Common;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Repository.Interfaces
{
    public interface IWorkRecordRepository
    {
       public Task AddWorkRecordAsync(WorkRecord model);

        public Task<List<WorkRecord>> AllWorkRecordsByEmployeeIdAsync(Guid id);

        public Task<List<WorkRecord>> AllWorkRecordsAsync();

        // Returns total Count with this settings and the records
        public Task<QueryPageWrapClass<WorkRecord>> GetWorkRecordsPageAndTotalCountAsync(QueryParameterClass parameters, Guid employeeId);

        public Task<int> WorkRecordsCountByEmployeeIdAsync(Guid employeeId);

        public Task DeleteById(Guid id);
    }
}
