using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMS.Data.Models;

namespace HCMS.Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByRoleName(string roleName);
    }
}
