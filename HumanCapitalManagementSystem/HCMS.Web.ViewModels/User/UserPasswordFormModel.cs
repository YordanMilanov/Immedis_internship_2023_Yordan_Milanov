using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.User;

namespace HCMS.Web.ViewModels.User
{
    public class UserPasswordFormModel
    {
        [Required]
        [Display(Name = "Password")]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = "Password length must be between {2} and {1} symbols.")]
        public string Password { get; set; } = null!;

        [Required]
        [Display(Name = "Repeat your password")]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = "Password length must be between {2} and {1} symbols.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
