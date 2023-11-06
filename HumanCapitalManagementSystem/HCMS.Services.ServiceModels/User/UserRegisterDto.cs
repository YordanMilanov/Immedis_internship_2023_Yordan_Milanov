using HCMS.Common.Structures;

namespace HCMS.Services.ServiceModels.User
{
    public class UserRegisterDto
    {
        public Name Username { get; set; }

        public Email Email { get; set; }

        public Password Password { get; set; }
    }
}
