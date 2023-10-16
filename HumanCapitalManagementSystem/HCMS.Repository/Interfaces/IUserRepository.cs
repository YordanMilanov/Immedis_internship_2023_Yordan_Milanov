using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Data.Models;
using HCMS.Web.ViewModels.User;

namespace HCMS.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task RegisterUser(User user);

        Task<bool> UserExistsByUsername(string username);
        Task<bool> UserExistsByEmail(string email);

        Task<UserLoginFormModel> GetUserByUsername(string username);
    }
}
