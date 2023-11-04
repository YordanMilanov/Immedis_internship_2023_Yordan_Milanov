using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models
{
    [Table("UsersClaims")]
    public class UserClaim
    {
        public UserClaim()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string ClaimType { get; set; } = null!;
        public string ClaimValue { get; set; } = null!;
    }
}
