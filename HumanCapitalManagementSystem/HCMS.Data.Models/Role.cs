using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HCMS.Common;

namespace HCMS.Data.Models
{
    [Table("Roles")]

    public class Role
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(DataModelConstants.Role.RoleNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(DataModelConstants.VarcharDefaultLength)]
        public string Description { get; set; } = null!;

        public virtual ICollection<UserRole> UsersRoles { get; set; }
    }
}
