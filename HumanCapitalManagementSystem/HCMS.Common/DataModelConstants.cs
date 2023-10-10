using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMS.Common
{
    public static class DataModelConstants
    {
        public static class User
        {
            public const int UsernameMaxLength = 50;
            public const int UsernameMinLength = 5;
            public const int PasswordMaxLength = 50;
            public const int PasswordMinLength = 5;
            public const int EmailMaxLength = 50;
            public const int EmailMinLength = 5;
        }
    }
}
