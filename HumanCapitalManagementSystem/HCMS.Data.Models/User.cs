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
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public virtual ICollection<UserRole> UsersRoles { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
    }
}
