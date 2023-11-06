using HCMS.Services.ServiceModels.User;

namespace HCMS.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(UserRegisterDto model);

        public Task<bool> IsUsernameExists(string username);
        public Task<bool> IsEmailExists(string email);

        public Task<bool> IsPasswordMatchByUsername(string username, string password);

        public Task<UserDto> GetUserDtoByUsername(string username);

        public Task<UserViewDto> GetUserViewDtoById(Guid id);
    }
}
