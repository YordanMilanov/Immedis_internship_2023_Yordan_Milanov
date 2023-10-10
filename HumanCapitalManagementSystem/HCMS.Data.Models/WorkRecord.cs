using System.ComponentModel.DataAnnotations;
using HCMS.Common;

namespace HCMS.Data.Models
{
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
        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public Guid CompanyId { get; set; }


        public Guid EmployeeId { get; set; }
    }
}
//"Id" VARCHAR PRIMARY KEY, --GUID
//"Position" VARCHAR(50) NOT NULL,
//"Department" VARCHAR(50) NULL,
//"Salary" DECIMAL(10, 2) NOT NULL,
//"StartDate" DATE NOT NULL,
//"EndDate" DATE NULL, --If Null - currently at that company
//"CompanyId" VARCHAR NOT NULL,
//"EmployeeId" VARCHAR NOT NULL,