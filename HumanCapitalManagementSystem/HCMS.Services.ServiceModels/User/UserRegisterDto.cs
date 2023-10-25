using HCMS.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMS.Services.ServiceModels.User
{
    public class UserRegisterDto
    {
        public Name Username { get; set; }

        public Email Email { get; set; }

        public Password Password { get; set; }
    }
}
