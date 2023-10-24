using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HCMS.Common.Structures;
using HCMS.Data.Models;

namespace HCMS.Services.ServiceModels
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
