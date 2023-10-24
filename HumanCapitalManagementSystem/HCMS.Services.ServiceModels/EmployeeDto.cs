using HCMS.Common.Structures;

namespace HCMS.Services.ServiceModels
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string? PhotoUrl { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime AddDate { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? UserId { get; set; }

        public string address { get; set; }
        public string address { get; set; }
        public string address { get; set; }
    }
}
