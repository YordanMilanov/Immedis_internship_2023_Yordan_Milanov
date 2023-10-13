using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HCMS.Common;

namespace HCMS.Data.Models
{
    [Table("Employees")]

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
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime AddDate { get; set; } = DateTime.Now;

        //FKs
        public Guid? CompanyId {get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public Guid? LocationId {get; set; }
      
        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set;}
    }
}