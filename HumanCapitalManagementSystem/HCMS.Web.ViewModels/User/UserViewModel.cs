namespace HCMS.Web.ViewModels.User
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
    }
}
