using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IWorkRecordRepository
    {
       public Task AddWorkRecord(WorkRecord model);
    }
}
