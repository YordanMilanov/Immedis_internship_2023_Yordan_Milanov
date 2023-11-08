using HCMS.Web.ViewModels.BaseViewModel;

namespace HCMS.Web.ViewModels.User
{
    public class UserQueryModel : PageQueryModel
    {
        public UserQueryModel()
        {
            this.CurrentPage = 1;
            this.ItemsPerPage = 10;

            this.Items = new List<UserViewModel>();
        }

        public IEnumerable<UserViewModel> Items { get; set; }
    }
}
