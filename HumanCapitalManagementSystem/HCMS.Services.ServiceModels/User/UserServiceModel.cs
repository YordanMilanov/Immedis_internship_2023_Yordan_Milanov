using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HCMS.Data.Models;

namespace HCMS.Services.ServiceModels.User
{
    public class UserServiceModel
    {
        public UserServiceModel()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Username { get; set; } = null!;

        public ICollection<Role> Roles { get; set; }
    }
}
