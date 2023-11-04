using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Data.Models
{
    [Table("Locations")]

    public class Location
    {
        public Location()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Country {get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerType { get; set; }
    }
}
