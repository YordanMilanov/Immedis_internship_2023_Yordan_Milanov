using System.ComponentModel.DataAnnotations;
using HCMS.Common;

namespace HCMS.Data.Models
{
    public class Role
    {
        public Role()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DataModelConstants.Role.RoleNameMaxLength)]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}

//"Id" INT PRIMARY KEY,
//"Name" VARCHAR(20) NOT NULL UNIQUE,
//"Description" TEXT NULL,
