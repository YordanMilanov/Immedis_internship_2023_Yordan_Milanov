using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IUserRoleRepository
    {
        public Task AddUserRoleAsync(UserRole userRole);
        public Task RemoveUserRoleAsync(UserRole userRole);
    }
}
