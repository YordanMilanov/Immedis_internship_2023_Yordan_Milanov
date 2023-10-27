using System.ComponentModel.DataAnnotations;
namespace HCMS.Web.ViewModels.Company
{
    public class CompanySelectViewModel
    {
        [Required]
        [Display(Name = "Company name")]
        public string Name { get; set; } = null!;
    }
}
