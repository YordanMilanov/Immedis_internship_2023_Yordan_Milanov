namespace HCMS.Services.ServiceModels.User
{
    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
