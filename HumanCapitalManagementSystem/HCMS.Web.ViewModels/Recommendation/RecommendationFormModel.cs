using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.Recommendation;

namespace HCMS.Web.ViewModels.Recommendation
{
    public class RecommendationFormModel
    {
        public Guid RecommenderId { get; set; }

        [Required]
        [MinLength(DescriptionMinLength, ErrorMessage = "Description length must be at least {1} characters long!")]
        [Display(Name = "Letter")]
        public string Description { get; set; } = null!;

        [EmailAddress]
        [Display(Name = "Employee Email")]
        public string EmployeeEmail { get; set; } = null!;

        [Required]
        [Display(Name = "Receiving company")]
        public string CompanyName { get; set; } = null!;
    }
}
