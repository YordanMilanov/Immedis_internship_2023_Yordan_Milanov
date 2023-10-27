using HCMS.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
