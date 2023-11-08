using HCMS.Common;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;

namespace HCMS.Repository.Interfaces
{
    public interface IUserRepository

    {
    Task RegisterUser(User user);

    Task<bool> UserExistsByUsername(string username);
    Task<bool> UserExistsByEmail(string email);
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByUsernameAsync(string username);
    Task UpdateUserAsync(User user);
    Task<QueryPageWrapClass<User>> GetUserCurrentPageAsync(QueryParameterClass parameters);
    }
}
