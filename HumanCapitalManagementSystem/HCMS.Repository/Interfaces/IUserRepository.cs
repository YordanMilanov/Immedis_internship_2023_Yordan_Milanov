using HCMS.Data.Models;
using HCMS.Services.ServiceModels.User;

namespace HCMS.Repository.Interfaces
{
    public interface IUserRepository

    {
    Task RegisterUser(User user);

    Task<bool> UserExistsByUsername(string username);
    Task<bool> UserExistsByEmail(string email);

    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByUsernameAsync(string username);

    }
}
