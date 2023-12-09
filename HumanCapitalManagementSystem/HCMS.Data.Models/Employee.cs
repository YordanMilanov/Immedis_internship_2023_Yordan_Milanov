using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models
{
    [Table("Employees")]

    public class Employee
    {
        public Employee()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? PhotoUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime AddDate { get; set; } = DateTime.Now;
        public Guid? CompanyId {get; set; }
        public virtual Company? Company { get; set; }
        public Guid? UserId { get; set; }
        public virtual User? User { get; set; }
        public Guid? LocationId {get; set; }
        public virtual Location? Location { get; set;}
        public virtual ICollection<WorkRecord>? WorkRecords { get; set; }
        public virtual ICollection<Education>? Educations { get; set; }
        public virtual ICollection<Application>? Applications { get; set; }
    }
}