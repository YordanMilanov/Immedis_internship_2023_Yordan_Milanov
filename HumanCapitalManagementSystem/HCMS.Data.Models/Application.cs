﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HCMS.Common;

namespace HCMS.Data.Models
{
    [Table("Applications")]
    public class Application
    {
        public Application()
        {
            this.Id = Guid.NewGuid();
        }

        [Key] 
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DataModelConstants.Application.PositionMaxLength)]
        public string Position { get; set; } = null!;
        [Required]
        [MaxLength(DataModelConstants.Application.DepartmentMaxLength)]
        public string Department { get; set; } = null!;
        
        [Required]
        public string CoverLetter { get; set; } = null!;
        
        [Required]
        public DateTime AddDate { get; set; } = DateTime.Now;

        [Required]
        public Guid ToCompanyId { get; set; }
        [ForeignKey("ToCompanyId")]
        public Company Company { get; set; } = null!;
       
        [Required]
        public Guid FromEmployeeId { get; set; }

        [ForeignKey("FromEmployeeId")]
        public Employee Employee { get; set; } = null!;
    }
}

//"Id" VARCHAR PRIMARY KEY, --GUID
//"Position" VARCHAR(50) NOT NULL,
//"Department" VARCHAR(50) NOT NULL,
//"CoverLetter" TEXT NULL,
//"ToCompanyId" VARCHAR NOT NULL,
//"FromEmployeeId" VARCHAR NOT NULL,