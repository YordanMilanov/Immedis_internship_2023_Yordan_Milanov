using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.User;
using HCMS.Common;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.ServiceModels.User;

namespace HCMS.Services;
using BCrypt.Net;

public class UserService : IUserService
{
    private readonly IRoleRepository roleRepository;
    private readonly IUserRepository userRepository;

    public UserService(IRoleRepository roleRepository, IUserRepository userRepository)
    {
        this.roleRepository = roleRepository;
        this.userRepository = userRepository;
    }

    public async Task RegisterUserAsync(UserRegisterFormModel formModel)
    {
        Role role = await roleRepository.GetRoleByRoleName(RoleConstants.EMPLOYEE); //EMPLOYEE - ROLE

        string password = HashPassword(formModel.Password);

        User user = new User
        {
            Username = formModel.Username,
            Password = password,
            Email = formModel.Email,
            RegisterDate = DateTime.UtcNow.AddHours(3),
            UserClaims = new List<UserClaim>()
        };
        UserRole userRole = new UserRole()
        {
            Role = role,
            RoleId = role.Id,
            UserId = user.Id,
            User = user,
        };
        UserClaim userClaim = new UserClaim
        {
            UserId = user.Id,
            ClaimType = "ROLE",
            ClaimValue = "EMPLOYEE"
        };
        user.UsersRoles.Add(userRole);
        user.UserClaims.Add(userClaim);

        try
        {
            await userRepository.RegisterUser(user);
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }

    public async Task<bool> IsUsernameExists(string username)
    {
        try
        {
            bool isExists = await userRepository.UserExistsByUsername(username);
            return isExists;
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }

    public async Task<bool> IsEmailExists(string email)
    {
        return await userRepository.UserExistsByEmail(email);

    }

    public async Task<bool> IsPasswordMatchByUsername(string username, string password)
    {
        try
        {
            //Caught below if it throws
            UserLoginFormModel useLoginModel = await userRepository.GetUserLoginFormModelByUsername(username);

            if (VerifyPassword(useLoginModel.Password, password))
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            //throws if user not found
            throw new Exception("Unexpected error occurred. The staff are working on the problem!");
        }
    }

    public async Task<UserServiceModel> GetUserServiceModelByUsername(string username)
    {
       return await userRepository.GetUserServiceModelByUsername(username);
    }

    public string HashPassword(string plainPassword)
    {
        return BCrypt.HashPassword(plainPassword);
    }

    public bool VerifyPassword(string hashedPassword, string providedPlainPassword)
    {
        return BCrypt.Verify(providedPlainPassword, hashedPassword);
    }

}
