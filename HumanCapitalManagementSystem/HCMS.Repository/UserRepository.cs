using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task RegisterUser(User user)
        { 
            await dbContext.Users.AddAsync(user);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<bool> UserExistsByUsername(string username)
        {
            return await this.dbContext.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> UserExistsByEmail(string email)
        {
            return await this.dbContext.Users.AnyAsync(u => u.Email == email);
        }
    }
}
