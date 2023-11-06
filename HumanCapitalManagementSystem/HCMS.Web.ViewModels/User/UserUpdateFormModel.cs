using HCMS.Common.CustomValidationAnnotation;
using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.User;

namespace HCMS.Web.ViewModels.User
{
    public class UserUpdateFormModel
    {
        public Guid Id { get; set; }


        [Required]
        [Display(Name = "Username")]
        [StringLength(UsernameMaxLength, MinimumLength = UsernameMinLength, ErrorMessage = "Username length must be between {2} and {1} symbols.")]
        [NoWhitespace(ErrorMessage = "Whitespace is not allowed!")]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email")]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength, ErrorMessage = "Email length must be between {2} and {1} symbols.")]
        public string Email { get; set; } = null!;
    }
}
