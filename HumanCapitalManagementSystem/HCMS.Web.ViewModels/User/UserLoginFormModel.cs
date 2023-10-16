using HCMS.Infrastructure.CustomValidationAnnotation;
using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.User;


namespace HCMS.Web.ViewModels.User
{

    public class UserLoginFormModel
    {
        [Required]
        [Display(Name = "Username")]
        [StringLength(UsernameMaxLength, MinimumLength = UsernameMinLength, ErrorMessage = "Username length must be between {2} and {1} symbols.")]
        [NoWhitespace(ErrorMessage = "Whitespace is not allowed!")]
        public string Username { get; set; } = null!;

        [Required]
        [Display(Name = "Password")]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = "Password length must be between {2} and {1} symbols.")]
        public string Password { get; set; } = null!;
    }
}
