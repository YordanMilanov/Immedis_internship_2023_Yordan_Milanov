using HCMS.Services.ServiceModels;
using HCMS.Web.ViewModels.User;

namespace HCMS.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(UserRegisterFormModel formModel);

        public Task<bool> IsUsernameExists(string username);
        public Task<bool> IsEmailExists(string email);

        public Task<bool> IsPasswordMatchByUsername(string username, string password);

        public Task<UserDto> GetUserServiceModelByUsername(string username);
    }
}
