namespace HCMS.Services.ServiceModels.User
{
    public class UserDto
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; } = null!;

        public string? Email { get; set; } = null!;

        public string? Password { get; set; } = null!;

        public List<string>? Roles { get; set; }

        public Guid? EmployeeId { get; set; }
    }
}
