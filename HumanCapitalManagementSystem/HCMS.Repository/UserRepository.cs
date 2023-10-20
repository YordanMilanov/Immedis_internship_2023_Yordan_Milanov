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

        public async Task<User?> GetUserById(Guid id)
        {
            User user = new User();
            try
            {
                user = await dbContext.Users
                    .Where(user => user.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }

            return user;
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
                .Where(u => u.Username == username)
                .Include(u => u.UsersRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => new UserServiceModel()
                {
                    Id = u.Id, // Remove the Guid.Parse conversion
                    Username = u.Username,
                    Roles = u.UsersRoles
                        .Select(ur => ur.Role)
                        .ToList()
                })
                .FirstAsync();
        }
    }
}
