using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HCMS.Repository.Implementation
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task RegisterUserAsync(User user)
        {
            if (await this.dbContext.Users.AnyAsync(u => u.Username == user.Username))
            {
                throw new ConstraintException("Username is already used!");
            }
            else if (await this.dbContext.Users.AnyAsync(u => u.Email == user.Email))
            {
                throw new ConstraintException("Email is already used!");
            }

            await dbContext.Users.AddAsync(user);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.Message.Contains("UC_Username_Unique"))
                {
                    throw new ConstraintException("Username is already used!");
                }
                else if (ex.Message.Contains("UC_Email_Unique"))
                {
                    throw new ConstraintException("Email is already used!");
                }
                throw;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("User with this Id already exists");
            }
        }

        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            try
            {
                return await dbContext.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> UserExistsByEmailAsync(string email)
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
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"User with Id: {id} does not exists!");
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
            catch(InvalidOperationException)
            {
                throw new InvalidOperationException($"User with username: {username} does not exists!");
            }
        }

        public async Task UpdateUserAsync(User model)
        {
            try
            {


                if(await this.dbContext.Users.AnyAsync(u => u.Username == model.Username && u.Id != model.Id)) {
                    throw new ConstraintException("Username is already used!");
                } 
                else if(await this.dbContext.Users.AnyAsync(u => u.Email == model.Email && u.Id != model.Id))
                {
                    throw new ConstraintException("Email is already used!");
                }
                else if (await this.dbContext.Users.AnyAsync(u => u.Id == model.Id) == false)
                {
                    throw new MissingPrimaryKeyException("No user with the current id was found!");
                }
                User user = await this.dbContext.Users.FirstAsync(u => u.Id == model.Id);
                user.Username = model.Username;
                user.Email = model.Email;
                await dbContext.SaveChangesAsync();
            } 
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<QueryPageWrapClass<User>> GetUserCurrentPageAsync(QueryParameterClass parameters)
        {
            try
            {
                IQueryable<User> query = dbContext.Users.Include(u => u.UsersRoles).ThenInclude(ur => ur.Role);

                //check the search string
                if (parameters.SearchString != null)
                {
                    query = query.Where(u =>
                    u.Username.Contains(parameters.SearchString) ||
                    u.Email.Contains(parameters.SearchString) ||
                    u.UsersRoles.Any(ur => ur.Role.Name.Contains(parameters.SearchString)));
                }

                int countOfElements = query.Count();

                //check the order
                switch (parameters.OrderPageEnum)
                {
                    case OrderPageEnum.Newest:
                        query = query.OrderBy(e => e.RegisterDate);
                        break;
                    case OrderPageEnum.Oldest:
                        query = query.OrderByDescending(e => e.RegisterDate);
                        break;
                }

                //Pagination
                int usersCountToSkip = (parameters.CurrentPage - 1) * parameters.ItemsPerPage;
                query = query.Skip(usersCountToSkip).Take(parameters.ItemsPerPage);

                List<User> users = await query.ToListAsync();

                QueryPageWrapClass<User> result = new QueryPageWrapClass<User>()
                {
                    TotalItems = countOfElements,
                    Items = users
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            };
        }
    }
}
