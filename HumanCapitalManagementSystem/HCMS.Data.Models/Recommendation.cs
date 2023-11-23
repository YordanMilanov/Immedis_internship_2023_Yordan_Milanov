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
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public DateTime RecommendDate { get; set; } = DateTime.Now;
        public Guid FromEmployeeId { get; set; }
        public virtual Employee FromEmployee { get; set; } = null!;
        public Guid ForEmployeeId { get; set; }
        public virtual Employee ForEmployee { get; set; } = null!;
        public Guid ToCompanyId { get; set; }
        public virtual Company Company { get; set; } = null!;

    }
}
