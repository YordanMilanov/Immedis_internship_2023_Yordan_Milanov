using System.ComponentModel;

namespace HCMS.Web.ViewModels.Company
{
    public class CompanyQueryModel
    {
        public CompanyQueryModel()
        {
            this.CurrentPage = 1;
            this.CompaniesPerPage = 3;

            this.Companies = new List<CompanyViewModel>();
        }

        [DisplayName("Search by word")]
        public string? SearchString { get; set; }
        public int CurrentPage { get; set; }

        public int TotalCompanies { get; set; }

        [DisplayName("Per page")]
        public int CompaniesPerPage { get; set; }

        public IEnumerable<CompanyViewModel> Companies { get; set; }
    }
}
