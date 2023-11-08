using HCMS.Web.ViewModels.BaseViewModel;

namespace HCMS.Web.ViewModels.Employee
{
    public class EmployeeQueryModel
    {
        public EmployeeQueryModel() : base()
        {
            Items = new List<EmployeeViewModel>();
        }

        public IEnumerable<EmployeeViewModel> Items { get; set; }
    }
}
