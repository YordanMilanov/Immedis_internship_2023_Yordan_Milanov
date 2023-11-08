using HCMS.Common;

namespace HCMS.Web.ViewModels.User
{
    public class UserRoleFormModel
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = null!;
        public ActionEnum Action { get; set; }
    }
}
