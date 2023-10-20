using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.Location;

namespace HCMS.Web.ViewModels.Location
{
    public class LocationFormModel
    {
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
