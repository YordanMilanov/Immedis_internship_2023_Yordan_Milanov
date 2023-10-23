using HCMS.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HCMS.Web.ViewModels.Company
{
    public class CompanyViewModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string IndustryField { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
