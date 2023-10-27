using HCMS.Common.Structures;

namespace HCMS.Services.ServiceModels.Employee
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }

        public Name FirstName { get; set; }

        public Name LastName { get; set; }
        public Email Email { get; set; }

        public Phone PhoneNumber { get; set; }

        public string? PhotoUrl { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime AddDate { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? UserId { get; set; }

        public LocationStruct Location { get; set; }
    }
}
