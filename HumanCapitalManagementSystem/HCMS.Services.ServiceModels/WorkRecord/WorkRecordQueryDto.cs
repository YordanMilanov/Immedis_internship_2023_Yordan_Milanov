using HCMS.Common;

namespace HCMS.Services.ServiceModels.WorkRecord
{
    public class WorkRecordQueryDto
    {
        public string? SearchString { get; set; }
        public OrderPageEnum OrderPageEnum { get; set; }
    }
}
