using HCMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMS.Data.Models
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DataModelConstants.VarcharDefaultLength)]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(DataModelConstants.Company.IndustryFieldMaxLength)]
        public string IndustryField { get; set; } = null!;

        public Guid? LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }
    }
}

//"Id" VARCHAR PRIMARY KEY, --GUID
//"Name" VARCHAR(20) NOT NULL UNIQUE,
//"Description" TEXT NOT NULL,
//"IndustryField" VARCHAR(20) NOT NULL,
//"LocationId" VARCHAR NOT NULL,
