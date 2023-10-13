using System.ComponentModel.DataAnnotations;
using static HCMS.Common.DataModelConstants.User;


namespace HCMS.Web.ViewModels.User
{
    public class UserRegisterFormModel
    {

        [Required]
        [Display(Name = "Username")]
        [StringLength(UsernameMaxLength, MinimumLength = UsernameMinLength)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "Password")]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string Password { get; set; } = null!;

        [Required]
        [Display(Name = "Repeat your password")]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
