using HCMS.Services.ServiceModels.BaseClasses;

namespace HCMS.Services.ServiceModels.WorkRecord
{
    public class WorkRecordQueryDto : QueryDtoResult<WorkRecordDto>
    {
        public Guid EmployeeId { get; set; }
    }
}
