using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.ServiceModels.User;
using Microsoft.EntityFrameworkCore;
using HCMS.Web.ViewModels.User;

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
            return await dbContext.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> UserExistsByEmail(string email)
        {
            return await dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<UserLoginFormModel> GetUserLoginFormModelByUsername(string username)
        {

            UserLoginFormModel user = await dbContext.Users
                .Where(u => u.Username.ToLower() == username.ToLower())
                .Select(u => new UserLoginFormModel()
                {
                    Username = u.Username,
                    Password = u.Password
                }).FirstAsync();
            return user;
        }

        public async Task<UserServiceModel> GetUserServiceModelByUsername(string username)
        {
            return await this.dbContext
                .Users
                .Select(u => new UserServiceModel()
                {
                    Id = Guid.Parse(u.Id.ToString()),
                    Username = u.Username,
                    MaxRole = u.UsersRoles
                        .OrderBy(ur => ur.Role.Id)
                        .Select(ur => ur.Role)
                        .First()
                }).FirstAsync();
        }
    }
}
