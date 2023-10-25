using HCMS.Common.Structures;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using HCMS.Web.ViewModels.User;
using HCMS.Services.ServiceModels.User;

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

        public async Task<User?> GetUserById(Guid id)
        {
            User? user = new User();
            try
            {
                user = await dbContext.Users
                    .Where(user => user.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return user;
        }

        public async Task<UserDto> GetUserDtoByUsername(string username)
        {
            UserDto? user = await dbContext.Users
                .Where(u => u.Username.ToLower() == username.ToLower())
                .Select(u => new UserDto()
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Password = u.Password,
                    Roles = new List<string>()
                }).FirstOrDefaultAsync();
            if (user != null)
            {
                List<Role> roles = await dbContext.UsersRoles
               .Where(ur => ur.User.Id == user.Id)
               .Select(ur => ur.Role).ToListAsync();

                foreach (Role role in roles)
                {
                    user.Roles!.Add(role.Name);
                }
                return user;
            }
            return null!;
        }
    }
}
