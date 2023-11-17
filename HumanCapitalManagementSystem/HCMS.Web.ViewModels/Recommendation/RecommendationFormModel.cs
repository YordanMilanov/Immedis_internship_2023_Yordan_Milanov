using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.Recommendation;

namespace HCMS.Web.ViewModels.Recommendation
{
    public class RecommendationFormModel
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [EmailAddress]
        public string EmployeeEmail { get; set; } = null!;

        public string CompanyName { get; set; }

    }
}
