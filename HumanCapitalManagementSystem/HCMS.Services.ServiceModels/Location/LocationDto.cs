using HCMS.Common;
using System.ComponentModel.DataAnnotations;

namespace HCMS.Services.ServiceModels.Location
{
    public class LocationDto
    {
        public Guid Id { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
    }
}
