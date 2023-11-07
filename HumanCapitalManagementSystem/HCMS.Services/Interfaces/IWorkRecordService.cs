using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Services.Interfaces
{
    public interface IWorkRecordService
    {
        public Task AddWorkRecordAsync(WorkRecordDto model);

        public Task<List<WorkRecordDto>> GetAllWorkRecordsDtosByEmployeeIdAsync(Guid id);

        public Task<List<WorkRecordDto>> GetAllWorkRecordsDtosAsync();

        public Task<(int, List<WorkRecordDto>)> GetWorkRecordsPageAndTotalCountAsync(WorkRecordQueryDto searchModel);

        public Task<int> GetWorkRecordsCountByEmployeeIdAsync(Guid employeeId);
        public Task DeleteById(Guid id);
    }
}