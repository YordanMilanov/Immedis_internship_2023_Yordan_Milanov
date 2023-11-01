using HCMS.Services.Interfaces;
using HCMS.Common;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;

namespace HCMS.Services.Implementation;
using BCrypt.Net;
using HCMS.Services.ServiceModels.User;

internal class UserService : IUserService
{
    private readonly IRoleRepository roleRepository;
    private readonly IUserRepository userRepository;

    public UserService(IRoleRepository roleRepository, IUserRepository userRepository)
    {
        this.roleRepository = roleRepository;
        this.userRepository = userRepository;
    }

    public async Task RegisterUserAsync(UserRegisterDto formModel)
    {
        Role role = await roleRepository.GetRoleByRoleName(RoleConstants.EMPLOYEE); //EMPLOYEE - ROLE

        string password = BCrypt.HashPassword(formModel.Password.ToString());

        User user = new User
        {
            Username = formModel.Username.ToString(),
            Password = password,
            Email = formModel.Email.ToString(),
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
            ClaimValue = RoleConstants.EMPLOYEE
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
            return await userRepository.UserExistsByUsername(username);
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
            UserDto userDto = await userRepository.GetUserDtoByUsername(username);

            return VerifyPassword(userDto.Password!.ToString(), password);
        }
        catch (Exception)
        {
            //throws if user not found
            throw new Exception("Unexpected error occurred. The staff are working on the problem!");
        }
    }

    public async Task<UserDto> GetUserDtoByUsername(string username)
    {
        return await userRepository.GetUserDtoByUsername(username);
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
