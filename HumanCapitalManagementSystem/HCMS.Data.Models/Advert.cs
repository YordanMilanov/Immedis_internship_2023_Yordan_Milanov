using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models
{

    [Table("Adverts")]
    public class Advert
    {
        public Advert()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Position { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool RemoteOption { get; set; }
        public decimal Salary { get; set; }
        public DateTime AddDate { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; } = null!;
        public virtual ICollection<Application>? Applications { get; set; }
    }
}
