using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Services.Interfaces
{
    public interface IWorkRecordService
    {
        public Task AddWorkRecordAsync(WorkRecordDto model);

        public Task<List<WorkRecordDto>> GetAllWorkRecordsDtosByEmployeeId(Guid id);

        public Task<List<WorkRecordDto>> GetAllWorkRecordsDtosAsync();

        public Task<List<WorkRecordDto>> FilteredBySearchAndOrdered();
    }
}
