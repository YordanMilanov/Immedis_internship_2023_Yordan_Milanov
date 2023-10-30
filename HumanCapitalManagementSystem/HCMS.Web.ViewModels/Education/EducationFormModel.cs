using static HCMS.Common.DataModelConstants.Education;
using static HCMS.Common.DataModelConstants.Location;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HCMS.Web.ViewModels.Education
{
    public class EducationFormModel
    {

        public Guid Id { get; set; }

        [Required]
        [StringLength(UniversityMaxLength, MinimumLength = UniversityMinLength, ErrorMessage = "University name must be between {2} and {1} characters long")]
        public string University { get; set; } = null!;

        [Required]
        [StringLength(FieldOfEducationMaxLength, MinimumLength = FieldOfEducationMinLength, ErrorMessage = "Field of education must be between {2} and {1} characters long")]
        [DisplayName("Field of education")]
        public string FieldOfEducation { get; set; } = null!;

        [Required]
        public string Degree { get; set; } = null!;

        [Required]
        [Range(3, 6, ErrorMessage = "Grade must be between 3 and 6.")]
        public decimal Grade { get; set; }

        [Required]
        [DisplayName("Start date")]
        public DateTime StartDate { get; set; }
       
        [DisplayName("End date")]
        public DateTime? EndDate { get; set; }


        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength, ErrorMessage = "Address must be between {2} and {1} characters long")]
        public string? Address { get; set; }
       
        [Required]
        [StringLength(StateMaxLength, MinimumLength = StateMinLength, ErrorMessage = "State must be between {2} and {1} characters long")]
        public string State { get; set; } = null!;
        
        [Required]
        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength, ErrorMessage = "Country must be between {2} and {1} characters long")]
        public string Country { get; set; } = null!;
    }
}
