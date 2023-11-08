using HCMS.Common;
namespace HCMS.Services.ServiceModels.User
{
    public class UserRoleUpdateDto
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = null!;
        public ActionEnum Action { get; set; }
    }
}
