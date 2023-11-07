namespace HCMS.Web.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? CompanyName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public DateTime AddDate { get; set; }

        public string Country { get; set; } = null!;
        public string State { get; set; } = null!;
        public string? Address { get; set; } = null!;
    }
}
