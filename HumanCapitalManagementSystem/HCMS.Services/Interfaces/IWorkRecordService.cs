using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Services.Interfaces
{
    public interface IWorkRecordService
    {
        public Task AddWorkRecordAsync(WorkRecordDto model);

        public Task<List<WorkRecordDto>> GetAllWorkRecordsDtosByEmployeeIdAsync(Guid id);

        public Task<List<WorkRecordDto>> GetAllWorkRecordsDtosAsync();

        public Task<List<WorkRecordDto>> GetWorkRecordsPageAsync(WorkRecordQueryDto searchModel);

        public Task<int> GetWorkRecordsCountByEmployeeIdAsync(Guid employeeId);
    }
}