using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository.Implementation
{
    internal class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRoleRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddUserRoleAsync(UserRole model)
        {
            try
            {
                if(await dbContext.UsersRoles.AnyAsync(ur => ur.UserId == model.UserId && ur.RoleId == model.RoleId))
                {
                    throw new Exception("The user already is in this role");
                }
                await dbContext.UsersRoles.AddAsync(model);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveUserRoleAsync(UserRole model)
        {
            try
            {
                UserRole? userRole = await dbContext.UsersRoles.FirstOrDefaultAsync(ur => ur.UserId == model.UserId && ur.RoleId == model.RoleId)!;

                if (userRole != null)
                {
                    //check if this is the only role of the user:
                    int roleCount = await dbContext.UsersRoles
                    .Where(ur => ur.UserId == model.UserId)
                    .CountAsync();
                    if(roleCount <= 1) {
                        throw new Exception("The user cannot be left without any roles. The operation is denied!");
                    }
                    this.dbContext.UsersRoles.Remove(userRole);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"The user is not in the submited role!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
