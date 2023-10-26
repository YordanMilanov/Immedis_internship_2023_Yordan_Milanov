using HCMS.Common.JsonConverter;
using HCMS.Common.Structures;
using Newtonsoft.Json;

namespace HCMS.Services.ServiceModels
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? PhotoUrl { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime AddDate { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? UserId { get; set; }

        [JsonConverter(typeof(LocationConverter))]
        public LocationStruct Location { get; set; }
    }
}
