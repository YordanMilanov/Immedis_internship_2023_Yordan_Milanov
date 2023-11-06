namespace HCMS.Services.ServiceModels.User
{
    public class UserPasswordDto
    {
        public Guid Id { get; set; }
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
