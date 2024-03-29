﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.Employee;
using static HCMS.Common.DataModelConstants.Location;

namespace HCMS.Web.ViewModels.Employee
{
    public class EmployeeFormModel
    {

        public EmployeeFormModel()
        {
            AddDate = DateTime.Now;
        }

        public Guid Id { get; set; }

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

        [Display(Name = "Upload Photo")]
        [Required(AllowEmptyStrings = true)]
        public IFormFile? Photo { get; set; }

        [Required]
        [Display(Name = "Birth date")]
        public DateTime? DateOfBirth { get; set; }

        public DateTime AddDate { get; set; }

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

        //UserId
        public Guid UserId { get; set; }
    }
}
