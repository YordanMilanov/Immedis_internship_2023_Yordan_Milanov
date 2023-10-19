
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HCMS.Common.DataModelConstants.Employee;

namespace HCMS.Web.ViewModels.Employee
{
    public class EmployeeFormModel
    {

        public EmployeeFormModel()
        {
            AddDate = DateTime.Now;
        }

        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; set; } = null!;

        [Required]
        [Phone]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Photo link")]
        public string? PhotoUrl { get; set; }

        [Required]
        [Display(Name = "Birth date")]
        public DateTime DateOfBirth { get; set; }

        public DateTime AddDate { get; set; }
    }
}
