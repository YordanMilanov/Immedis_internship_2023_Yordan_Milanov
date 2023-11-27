using HCMS.Common.Structures;
using System.ComponentModel.DataAnnotations;

namespace HCMS.Services.ServiceModels.Advert
{
    public class AdvertAddDto
    {
        public Position Position { get; set; }
        public Department Department { get; set; }
        public Description Description { get; set; }
        public bool RemoteOption { get; set; }
        public Salary Salary { get; set; } 
        public DateTime AddDate { get; set; }
        public Guid CompanyId { get; set; }
    }
}
