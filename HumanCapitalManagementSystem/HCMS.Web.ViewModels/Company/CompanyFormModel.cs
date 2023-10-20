using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.Company;

namespace HCMS.Web.ViewModels.Company
{
    public class CompanyFormModel
    {
        [Required]
        [Display(Name = "Company name")]
        [StringLength(CompanyNameMaxLength, MinimumLength = CompanyNameMinLength, ErrorMessage = "Company name should be between {2} and {1} characters long")]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [Display(Name = "Industry field")]
        [StringLength(IndustryFieldMaxLength, MinimumLength = IndustryFieldMinLength, ErrorMessage = "Industry field should be between {2} and {1} characters long")]
        public string IndustryField { get; set; } = null!;
    }
}
