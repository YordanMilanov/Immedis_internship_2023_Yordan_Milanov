using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models
{
    [Table("Recommendations")]

    public class Recommendation
    {
        public Recommendation()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; } = null!;


        public DateOnly? RecommendDate { get; set; }

        [Required]
        public Guid ForEmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;

        [Required]
        public Guid ToCompanyId { get; set; }

        public virtual Company Company { get; set; } = null!;

    }
}
//"Id" VARCHAR PRIMARY KEY, --GUID
//"Description" TEXT NOT NULL,
//"RecommendDate" DATE NULL,
//"ForEmployeeId" VARCHAR NOT NULL,
//"ToCompanyId" VARCHAR NOT NULL,