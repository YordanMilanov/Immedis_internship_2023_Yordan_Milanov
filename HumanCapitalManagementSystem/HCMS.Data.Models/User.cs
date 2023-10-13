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
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DataModelConstants.User.UsernameMaxLength)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(DataModelConstants.User.PasswordMaxLength)]

        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(DataModelConstants.User.EmailMaxLength)]
        public string Email { get; set; } = null!;

        [Required]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [Required]
        public int RoleId { get; set; } 
        
        //Mapping entity field FKS


        public virtual Role Role { get; set; } = null!;
    }

    //"Id" VARCHAR PRIMARY KEY, --GUID
    //"Username" VARCHAR(50) NOT NULL UNIQUE,
    //"Email" VARCHAR(100) NOT NULL UNIQUE,
    //"Password" VARCHAR(50) NOT NULL,
    //"RoleId" INT NOT NULL,


}
