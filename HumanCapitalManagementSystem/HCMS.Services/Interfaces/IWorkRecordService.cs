using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Services.Interfaces
{
    public interface IWorkRecordService
    {
        public Task AddWorkRecordAsync(WorkRecordDto model);

        public Task<List<WorkRecordDto>> GetAllWorkRecordsDtosByEmployeeIdAsync(Guid id);

        public Task<List<WorkRecordDto>> GetAllWorkRecordsDtosAsync();

        public Task<QueryDtoResult<WorkRecordDto>> GetWorkRecordsPageAndTotalCountAsync(QueryDto model, Guid employeeId);

        public Task<int> GetWorkRecordsCountByEmployeeIdAsync(Guid employeeId);
        public Task DeleteById(Guid id);
    }
}