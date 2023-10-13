﻿using System.ComponentModel.DataAnnotations;
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