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
            public const int EmailMaxLength = 100;
            public const int EmailMinLength = 5;
        }

        public static class Role
        {
            public const int RoleNameMaxLength = 20;
        }

        public static class WorkRecord
        {
            public const int PositionMaxLength = 50;
            public const int PositionMinLength = 5;
            public const int DepartmentMaxLength = 50;
            public const int DepartmentMinLength = 5;
        }

        public static class Location
        {
            public const int AddressMaxLength = 50;
            public const int AddressMinLength = 5;
            public const int StateMaxLength = 50;
            public const int StateMinLength = 5;
            public const int CountryMaxLength = 50;
            public const int CountryMinLength = 5;
        }

        public static class Employee
        {
            public const int FirstNameMaxLength = 50;
            public const int FirstNameMinLength = 5;
            public const int LastNameMaxLength = 50;
            public const int LastNameMinLength = 5;
            public const int EmailMaxLength = 100;
            public const int EmailMinLength = 5;
            public const int PhoneNumberMaxLength = 50;
            public const int PhoneNumberMinLength = 8;
        }
    }
}
