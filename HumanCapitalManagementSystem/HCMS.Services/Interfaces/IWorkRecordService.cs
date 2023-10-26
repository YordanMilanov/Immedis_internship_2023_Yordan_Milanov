using HCMS.Services.ServiceModels.WorkRecord;

namespace HCMS.Services.Interfaces
{
    public interface IWorkRecordService
    {

        public Task AddWorkRecord(WorkRecordDto model);
    }
}
