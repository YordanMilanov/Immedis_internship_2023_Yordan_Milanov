namespace HCMS.Common
{
    public static class DataModelConstants
    {
        public const int VarcharDefaultLength = 255;

        public static class User
        {
            public const int UsernameMaxLength = 50;
            public const int UsernameMinLength = 3;
            public const int PasswordMaxLength = 50;
            public const int PasswordMinLength = 5;
            public const int EmailMaxLength = 100;
            public const int EmailMinLength = 5;
        }

        public static class Role
        {
            public const int RoleNameMaxLength = 20;
        }

        public static class Company
        {
            public const int CompanyNameMaxLength = 50;
            public const int CompanyNameMinLength = 3;
            public const int IndustryFieldMaxLength = 50;
            public const int IndustryFieldMinLength = 3;
        }

        public static class WorkRecord
        {
            public const int PositionMaxLength = 50;
            public const int PositionMinLength = 2;
            public const int DepartmentMaxLength = 50;
            public const int DepartmentMinLength = 2;
        }

        public static class Location
        {
            public const int AddressMaxLength = 50;
            public const int AddressMinLength = 2;
            public const int StateMaxLength = 50;
            public const int StateMinLength = 2;
            public const int CountryMaxLength = 50;
            public const int CountryMinLength = 2;
        }

        public static class Employee
        {
            public const int FirstNameMaxLength = 50;
            public const int FirstNameMinLength = 2;
            public const int LastNameMaxLength = 50;
            public const int LastNameMinLength = 2;
            public const int EmailMaxLength = 100;
            public const int EmailMinLength = 5;
            public const int PhoneNumberMaxLength = 50;
            public const int PhoneNumberMinLength = 8;
        }

        public static class Education
        {
            public const int UniversityMaxLength = 50;
            public const int UniversityMinLength = 2;
            public const int DegreeMaxLength = 50;
            public const int DegreeMinLength = 2;
            public const int FieldOfEducationMaxLength = 50;
            public const int FieldOfEducationMinLength = 2;
        }

        public static class Application
        {
            public const int PositionMaxLength = 50;
            public const int PositionMinLength = 2;
            public const int DepartmentMaxLength = 50;
            public const int DepartmentMinLength = 2;
        }
    }
}
