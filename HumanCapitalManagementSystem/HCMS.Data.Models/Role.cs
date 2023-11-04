using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models
{
    [Table("Roles")]

    public class Role
    {
        public Role()
        {
            this.UsersRoles = new List<UserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public virtual ICollection<UserRole> UsersRoles { get; set; }
    }
}
