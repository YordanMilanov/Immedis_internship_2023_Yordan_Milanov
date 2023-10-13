using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.User;
using BCrypt.Net;
using HCMS.Common;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Services;
using BCrypt.Net;

public class UserService : IUserService
{
    private readonly IRoleRepository roleRepository;
    private readonly IUserRepository userRepository;
    private readonly IUserRoleRepository userRoleRepository;

    public UserService(IRoleRepository roleRepository, IUserRepository userRepository, IUserRoleRepository userRoleRepository)
    {
        this.roleRepository = roleRepository;
        this.userRepository = userRepository;
        this.userRoleRepository = userRoleRepository;
    }

    public async Task RegisterUserAsync(UserRegisterFormModel formModel)
    {
        Role role = await roleRepository.GetRoleByRoleName(GeneralApplicationConstants.DefaultRegistrationRole); //USER

        string password = HashPassword(formModel.Password);

        User user = new User
        {
            Id = Guid.NewGuid(),
            Username = formModel.Username,
            Password = password,
            Email = formModel.Email,
            RegisterDate = DateTime.UtcNow.AddHours(3),
        };
        UserRole userRole = new UserRole()
        {
            Role = role,
            RoleId = role.Id,
            UserId = user.Id,
            User = user,
        };
        user.UsersRoles.Add(userRole);

        try
        {
            //await userRoleRepository.AddUserRoleAsync(userRole);
            await userRepository.RegisterUser(user);
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }

    public async Task<bool> IsUsernameExists(string username)
    {
        return await userRepository.UserExistsByUsername(username);
    }

    public async Task<bool> IsEmailExists(string email)
    {
        return await userRepository.UserExistsByEmail(email);

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
