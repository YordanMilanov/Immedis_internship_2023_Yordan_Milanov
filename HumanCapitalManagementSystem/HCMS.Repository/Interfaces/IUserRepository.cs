using HCMS.Common;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;

namespace HCMS.Repository.Interfaces
{
    public interface IUserRepository

    {
    Task RegisterUserAsync(User user);

    Task<bool> UserExistsByUsernameAsync(string username);
    Task<bool> UserExistsByEmailAsync(string email);
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByUsernameAsync(string username);
    Task UpdateUserAsync(User user);
    Task<QueryPageWrapClass<User>> GetUserCurrentPageAsync(QueryParameterClass parameters);
    }
}
