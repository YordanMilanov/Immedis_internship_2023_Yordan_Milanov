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
        [MaxLength(DataModelConstants.Education.DegreeMaxLength)]
        public string Degree { get; set; }

        [Required]
        [MaxLength(DataModelConstants.Education.FieldOfEducationMaxLength)]
        public string FieldOfEducation { get; set; }

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

//"Id" VARCHAR PRIMARY KEY, --GUID
//"StartDate" DATE NOT NULL,
//"EndDate" DATE NULL,
//"Degree" VARCHAR(50) NOT NULL,
//"FieldOfEducation" VARCHAR(50) NOT NULL,
//"Grade" DECIMAL(19,2) NOT NULL,
//"EmployeeId" VARCHAR NOT NULL,
//"LocationId" VARCHAR NULL,
