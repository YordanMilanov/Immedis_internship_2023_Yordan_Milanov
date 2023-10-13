using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HCMS.Common;

namespace HCMS.Data.Models
{
    [Table("WorkRecords")]

    public class WorkRecord
    {
        public WorkRecord()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DataModelConstants.WorkRecord.PositionMaxLength)]
        public string Position { get; set; } = null!;

        [MaxLength(DataModelConstants.WorkRecord.DepartmentMaxLength)]
        public string? Department { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required] 
        public DateTime AddDate { get; set; } = DateTime.Now;


        //FKs
        public Guid CompanyId { get; set; }
       
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public Guid EmployeeId { get; set; }
       
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}