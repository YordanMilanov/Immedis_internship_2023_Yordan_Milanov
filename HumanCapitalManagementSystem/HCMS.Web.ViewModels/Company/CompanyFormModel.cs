using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.Company;
using static HCMS.Common.DataModelConstants.Location;


namespace HCMS.Web.ViewModels.Company
{
    public class CompanyFormModel
    {

        public Guid Id { get; set; }

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

        //LocationStruct - Address
        [Required]
        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength, ErrorMessage = "Country name must be between {2} and {1} characters long")]
        public string Country { get; set; } = null!;

        [Required]
        [StringLength(StateMaxLength, MinimumLength = StateMinLength, ErrorMessage = "State name must be between {2} and {1} characters long")]
        public string State { get; set; } = null!;

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength, ErrorMessage = "Address must be between {2} and {1} characters long")]
        public string Address { get; set; } = null!;
    }
}
