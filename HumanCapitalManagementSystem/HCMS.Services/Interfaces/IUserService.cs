using HCMS.Web.ViewModels.User;

namespace HCMS.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(UserRegisterFormModel formModel);
    }
}
