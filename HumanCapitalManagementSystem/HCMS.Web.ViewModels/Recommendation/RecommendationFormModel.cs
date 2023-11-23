using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.Recommendation;

namespace HCMS.Web.ViewModels.Recommendation
{
    public class RecommendationFormModel
    {
        public Guid RecommenderId { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        [Display(Name = "Letter")]
        public string Description { get; set; } = null!;

        [EmailAddress]
        [Display(Name = "Employee Email")]
        public string EmployeeEmail { get; set; } = null!;

        [Display(Name = "Employee full name")]
        public string EmployeeName { get; set; } = null!;

        [Required]
        [Display(Name = "Receiving company")]
        public string CompanyName { get; set; } = null!;
    }
}
