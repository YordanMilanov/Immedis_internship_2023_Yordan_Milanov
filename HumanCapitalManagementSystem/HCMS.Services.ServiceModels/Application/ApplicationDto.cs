using HCMS.Services.ServiceModels.Advert;
using HCMS.Services.ServiceModels.Employee;

namespace HCMS.Services.ServiceModels.Application
{
    public class ApplicationDto
    {
        public string CoverLetter { get; set; } = null!;
        public DateTime AddDate { get; set; }
        public Guid FromEmployeeId{ get; set; }

        public EmployeeDto Employee { get; set; } = null!;
        public Guid AdvertId { get; set; }
        public AdvertViewDto Advert { get; set; } = null!;

    }
}
