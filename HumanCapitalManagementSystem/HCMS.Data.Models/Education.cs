using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HCMS.Common;

namespace HCMS.Data.Models
{
    [Table("Educations")]

    public class Education
    {
        public Education()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [MaxLength(DataModelConstants.Education.UniversityMaxLength)]
        public string University { get; set; } = null!;

        [Required]
        [MaxLength(DataModelConstants.Education.DegreeMaxLength)]
        public string Degree { get; set; } = null!;

        [Required]
        [MaxLength(DataModelConstants.Education.FieldOfEducationMaxLength)]
        public string FieldOfEducation { get; set; } = null!;

        [Required]
        public decimal Grade { get; set; }

        [Required]
        public Guid EmployeeId {get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; } = null!;

        public Guid? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }
    }
}
