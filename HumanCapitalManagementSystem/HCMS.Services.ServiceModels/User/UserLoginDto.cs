using HCMS.Common.JsonConverter;
using HCMS.Common.Structures;
using Newtonsoft.Json;

namespace HCMS.Services.ServiceModels.User
{
    public class UserLoginDto
    {
        [JsonConverter(typeof(NameConverter))]
        public Name Username { get; set; }

        [JsonConverter(typeof(PasswordConverter))]
        public Password Password { get; set; }
    }
}
