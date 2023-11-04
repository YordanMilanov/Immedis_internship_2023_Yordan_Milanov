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

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string IndustryField { get; set; } = null!;
        public Guid? LocationId { get; set; }
        public virtual Location? Location { get; set; }
    }
}
