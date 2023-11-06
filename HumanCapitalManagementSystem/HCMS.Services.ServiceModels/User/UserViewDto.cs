namespace HCMS.Services.ServiceModels.User
{
    public class UserViewDto
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public List<string>? Roles { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
