using HCMS.Web.ViewModels.Advert;
using HCMS.Web.ViewModels.Company;
using HCMS.Web.ViewModels.Employee;

namespace HCMS.Web.ViewModels.Application
{
    public class ApplicationViewModel : ApplicationFormModel
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public CompanyViewModel Company { get; set; } = null!;
        public EmployeeViewModel Employee { get; set; } = null!;
        public AdvertViewModel Advert { get; set; } = null!;
    }
}
