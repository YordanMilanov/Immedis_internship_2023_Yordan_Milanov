using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.User;

namespace HCMS.Web.ViewModels.User
{
    public class UserPasswordFormModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Current Password")]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = "Password length must be between {2} and {1} symbols.")]
        public string CurrentPassword { get; set; } = null!;

        [Required]
        [Display(Name = "New Password")]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = "Password length must be between {2} and {1} symbols.")]
        public string NewPassword { get; set; } = null!;

        [Required]
        [Display(Name = "Repeat new your password")]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = "Password length must be between {2} and {1} symbols.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
