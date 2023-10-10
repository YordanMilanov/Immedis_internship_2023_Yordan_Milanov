using System.ComponentModel.DataAnnotations;
using HCMS.Common;

namespace HCMS.Data.Models
{
    public class Employee
    {
        public Employee()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DataModelConstants.Employee.FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;
       
        [Required]
        [MaxLength(DataModelConstants.Employee.LastNameMaxLength)]

        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(DataModelConstants.Employee.EmailMaxLength)]

        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(DataModelConstants.Employee.PhoneNumberMaxLength)]

        public string PhoneNumber { get; set; } = null!;

        public string? PhotoUrl { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        public Guid? CompanyId {get; set; }

        public virtual Company? Company { get; set; }

        public Guid? UserId { get; set; }

        public virtual User? User { get; set; }

        public Guid? LocationId {get; set; }

        public virtual Location? Location { get; set;}
    }
}

//"Id" VARCHAR PRIMARY KEY, --GUID
//"FirstName" VARCHAR(50) NOT NULL,
//"LastName" VARCHAR(50) NOT NULL,
//"Email" VARCHAR(100) NOT NULL UNIQUE,
//"PhoneNumber" VARCHAR(50) NOT NULL UNIQUE,
//"PhotoURL" TEXT NULL,
//"DateOfBirth" DATE NOT NULL,
//"CompanyId" VARCHAR NULL,
//"UserId" VARCHAR NULL,
//"LocationId" VARCHAR NULL,