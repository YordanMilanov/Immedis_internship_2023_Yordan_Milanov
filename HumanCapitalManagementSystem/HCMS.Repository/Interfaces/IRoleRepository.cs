using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByRoleName(string roleName);
    }
}
