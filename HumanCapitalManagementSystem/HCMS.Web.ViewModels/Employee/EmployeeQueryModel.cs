using HCMS.Common;
using System.ComponentModel;

namespace HCMS.Web.ViewModels.Employee
{
    public class EmployeeQueryModel
    {
        public EmployeeQueryModel()
        {
            this.CurrentPage = 1;
            this.EmployeesPerPage = 3;

            this.Employees = new List<EmployeeViewModel>();
        }

        [DisplayName("Search by word")]
        public string? SearchString { get; set; }

        [DisplayName("Sort by")]
        public OrderPageEnum OrderPageEnum { get; set; }

        public int CurrentPage { get; set; }

        public int TotalEmployees { get; set; }

        [DisplayName("Per page")]
        public int EmployeesPerPage { get; set; }

        public IEnumerable<EmployeeViewModel> Employees { get; set; }
    }
}
