using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models
{
    [Table("Educations")]

    public class Education
    {
        public Education()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string University { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public string FieldOfEducation { get; set; } = null!;
        public decimal Grade { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;
        public Guid? LocationId { get; set; }
        public virtual Location? Location { get; set; }
    }
}
