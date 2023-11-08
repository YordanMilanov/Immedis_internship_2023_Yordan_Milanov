using HCMS.Common;
using System.ComponentModel;

namespace HCMS.Services.ServiceModels.User
{
    public class UserQueryDto
    {
        public UserQueryDto()
        {
            this.CurrentPage = 1;
            this.TotalEntities = 3;

            this.Users = new List<UserDto>();
        }

        [DisplayName("Search by word")]
        public string? SearchString { get; set; }

        [DisplayName("Sort by")]
        public OrderPageEnum OrderPageEnum { get; set; }

        public int CurrentPage { get; set; }

        public int TotalEntities { get; set; }

        [DisplayName("Per page")]
        public int PerPage { get; set; }

        public IEnumerable<UserDto> Users { get; set; }
    }
}
