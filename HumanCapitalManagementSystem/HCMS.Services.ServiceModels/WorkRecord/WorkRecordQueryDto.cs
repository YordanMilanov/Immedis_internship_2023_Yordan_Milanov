using HCMS.Common;

namespace HCMS.Services.ServiceModels.WorkRecord
{
    public class WorkRecordQueryDto
    {

        public string? SearchString { get; set; }

        public OrderPageEnum OrderPageEnum { get; set; }

        public int CurrentPage { get; set; }

        public int TotalWorkRecords { get; set; }

        public int WorkRecordsPerPage { get; set; }

        public IEnumerable<WorkRecordDto> WorkRecords { get; set; } = null!;

        public Guid EmployeeId { get; set; }
    }
}
