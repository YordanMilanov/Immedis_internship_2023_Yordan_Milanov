using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using HCMS.Services.ServiceModels.User;

namespace HCMS.Repository.Implementation
{
    internal class UserRepository : IUserRepository
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
            try
            {
                return await dbContext.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        public async Task<bool> UserExistsByEmail(string email)
        {
            try
            {
                return await dbContext.Users.AnyAsync(u => u.Email == email);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            try
            {
                return await dbContext.Users
                    .Include(u => u.UsersRoles)
                    .ThenInclude(ur => ur.Role)
                    .Where(user => user.Id == id)
                    .FirstAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await dbContext.Users
                .Where(u => u.Username.ToLower() == username.ToLower())
                .Include(u => u.UsersRoles)
                .ThenInclude(ur => ur.Role)
                .FirstAsync();
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUserAsync(User model)
        {
            try
            {
                User user = await this.dbContext.Users.FirstAsync(u => u.Id == model.Id);
                user.Username = model.Username;
                user.Email = model.Email;
                await dbContext.SaveChangesAsync();
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
