using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository.Implementation
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RoleRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<Role> GetRoleByRoleName(string roleName)
        {
            try
            {
                return await dbContext.Roles.FirstAsync(r => r.Name == roleName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
