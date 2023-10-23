using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HCMS.Common;

namespace HCMS.Data.Models
{
    [Table("Locations")]

    public class Location
    {
        public Location()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [MaxLength(DataModelConstants.Location.AddressMaxLength)]
        public string Address { get; set; }

        [Required]
        [MaxLength(DataModelConstants.Location.StateMaxLength)]
        public string State { get; set; }

        [Required]
        [MaxLength(DataModelConstants.Location.CountryMaxLength)]
        public string Country {get; set; }


    }
}
