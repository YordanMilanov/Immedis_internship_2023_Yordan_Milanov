using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Services.Interfaces;
using HCMS.Web.ViewModels.User;
using BCrypt.Net;

namespace HCMS.Services;
using BCrypt.Net;

public class UserService : IUserService
{

    public Task RegisterUserAsync(UserRegisterFormModel formModel)
    {
        throw new NotImplementedException();
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
