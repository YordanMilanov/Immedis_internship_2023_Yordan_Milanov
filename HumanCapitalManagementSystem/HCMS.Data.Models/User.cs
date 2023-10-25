using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HCMS.Common;
namespace HCMS.Data.Models
{
    [Table("Users")]
    public class User
    {

        public User()
        {
            this.Id = Guid.NewGuid();
            this.UsersRoles = new List<UserRole>();
            this.UserClaims = new List<UserClaim>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DataModelConstants.User.UsernameMaxLength)]
        [Column("Username")]
        public string Username { get; set; }

        [Required]
        [MaxLength(DataModelConstants.User.PasswordMaxLength)]
        [Column("Password")]

        public string Password { get; set; }

        [Required]
        [MaxLength(DataModelConstants.User.EmailMaxLength)]
        [Column("Email")]
        public string Email { get; set; }

        [Required]
        [Column("RegisterDate")]

        public DateTime RegisterDate { get; set; }

        public virtual ICollection<UserRole> UsersRoles { get; set; }

        public virtual ICollection<UserClaim> UserClaims { get; set; }


        // Computed property that calls the custom ToString method
    }
}
