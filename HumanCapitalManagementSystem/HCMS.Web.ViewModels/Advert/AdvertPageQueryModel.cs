using HCMS.Web.ViewModels.BaseViewModel;

namespace HCMS.Web.ViewModels.Advert
{
    public class AdvertPageQueryModel : PageQueryModel
    {
        public AdvertPageQueryModel() : base(1, 10) { }

        public bool RemoteOption { get; set; }
        public string? CompanyId { get; set; }
    }
}
