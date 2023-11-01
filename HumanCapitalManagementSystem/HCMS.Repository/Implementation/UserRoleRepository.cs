using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;

namespace HCMS.Repository.Implementation
{
    internal class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRoleRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddUserRoleAsync(UserRole userRole)
        {
            try
            {
                await dbContext.UsersRoles.AddAsync(userRole);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
    }
}
