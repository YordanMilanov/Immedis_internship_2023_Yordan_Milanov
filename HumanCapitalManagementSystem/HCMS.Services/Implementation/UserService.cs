﻿using HCMS.Common;
using HCMS.Data.Models;
using HCMS.Repository.Interfaces;
using HCMS.Services.Interfaces;

namespace HCMS.Services.Implementation;

using AutoMapper;
using BCrypt.Net;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Services.ServiceModels.BaseClasses;
using HCMS.Services.ServiceModels.User;

internal class UserService : IUserService
{
    private readonly IRoleRepository roleRepository;
    private readonly IUserRepository userRepository;
    private readonly IUserRoleRepository userRoleRepository;
    private readonly IMapper mapper;

    public UserService(IRoleRepository roleRepository, IUserRepository userRepository, IMapper mapper, IUserRoleRepository userRoleRepository)
    {
        this.roleRepository = roleRepository;
        this.userRepository = userRepository;
        this.userRoleRepository = userRoleRepository;
        this.mapper = mapper;
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
            await userRepository.RegisterUserAsync(user);
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
            return await userRepository.UserExistsByUsernameAsync(username);
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }

    public async Task<bool> IsEmailExists(string email)
    {
        return await userRepository.UserExistsByEmailAsync(email);

    }

    public async Task<bool> IsPasswordMatchByUsername(string username, string password)
    {
        try
        {
            //Caught below if it throws
            User user = await userRepository.GetUserByUsernameAsync(username);
            UserDto userDto = mapper.Map<UserDto>(user);

            return VerifyPassword(userDto.Password!.ToString(), password);
        }
        catch (Exception)
        {
            //throws if user not found
            throw new Exception("Unexpected error occurred. The staff are working on the problem!");
        }
    }

    public async Task<UserDto> GetUserDtoByUsernameAsync(string username)
    {
        User user = await userRepository.GetUserByUsernameAsync(username);
        UserDto userDto = mapper.Map<UserDto>(user);
        return userDto;
    }


    public async Task<UserViewDto> GetUserViewDtoByIdAsync(Guid id)
    {
        try
        {
            User user = await this.userRepository.GetUserByIdAsync(id);
            return mapper.Map<UserViewDto>(user);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task UpdateUserAsync(UserUpdateDto model)
    {
        try
        {
            User user = await this.userRepository.GetUserByIdAsync(model.Id);
            user.Username = model.Username;
            user.Email = model.Email;
            await this.userRepository.UpdateUserAsync(user);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task UpdatePasswordAsync(UserPasswordDto model)
    {
        User user = await this.userRepository.GetUserByIdAsync(model.Id);
        if (VerifyPassword(user.Password, model.CurrentPassword))
        {
            string newPassword = BCrypt.HashPassword(model.NewPassword);
            user.Password = newPassword;
        };
        await this.userRepository.UpdateUserAsync(user);
    }

    public async Task<QueryDtoResult<UserViewDto>> GetUsersCurrentPageAsync(QueryDto model)
    {
        try
        {
            QueryParameterClass parameters = mapper.Map<QueryParameterClass>(model);

            QueryPageWrapClass<User> pageItems = await this.userRepository.GetUserCurrentPageAsync(parameters);
            QueryDtoResult<UserViewDto> result = mapper.Map<QueryDtoResult<UserViewDto>>(pageItems);

            result.ItemsPerPage = model.ItemsPerPage;
            result.CurrentPage = model.CurrentPage;
            result.OrderPageEnum = model.OrderPageEnum;
            result.SearchString = model.SearchString;

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public bool VerifyPassword(string hashedPassword, string providedPlainPassword)
    {
        return BCrypt.Verify(providedPlainPassword, hashedPassword);
    }

    public async Task UpdateUserRoleAsync(UserRoleUpdateDto model)
    {
        try
        {
            Role role = await this.roleRepository.GetRoleByRoleName(model.Role);

            UserRole userRole = new UserRole()
            {
                UserId = model.Id,
                RoleId = role.Id
            };

            if (model.Action == ActionEnum.ADD)
            {
                await this.userRoleRepository.AddUserRoleAsync(userRole);
            }
            else if (model.Action == ActionEnum.REMOVE)
            {
                await this.userRoleRepository.RemoveUserRoleAsync(userRole);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
