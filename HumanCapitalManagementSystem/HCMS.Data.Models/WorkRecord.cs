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
        public Guid Id { get; set; }
        public string Position { get; set; } = null!;
        public string? Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime AddDate { get; set; } = DateTime.Now;
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}