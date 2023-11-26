using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models
{
    [Table("Applications")]
    public class Application
    {
        public Application()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string CoverLetter { get; set; } = null!;
        public DateTime AddDate { get; set; } = DateTime.Now;
        public Guid ToCompanyId { get; set; }
        public virtual Company Company { get; set; } = null!;
        public Guid FromEmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;
        public Guid AdvertId { get; set; }
        public virtual Advert Advert { get; set; } = null!;
    }
}
