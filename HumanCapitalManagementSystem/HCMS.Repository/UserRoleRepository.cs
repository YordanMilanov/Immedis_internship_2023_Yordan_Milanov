using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;

namespace HCMS.Repository
{
    public class UserRoleRepository : IUserRoleRepository
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
